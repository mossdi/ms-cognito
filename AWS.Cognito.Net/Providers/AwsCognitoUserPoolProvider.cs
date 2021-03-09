// <copyright file="AwsCognitoUserPoolProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Amazon;
    using Amazon.CognitoIdentity;
    using Amazon.CognitoIdentityProvider;
    using Amazon.CognitoIdentityProvider.Model;
    using Amazon.Extensions.CognitoAuthentication;
    using AWS.Cognito.Net.Interfaces.Providers;
    using AWS.Cognito.Net.Models;
    using Microsoft.Extensions.Configuration;

    public class AwsCognitoUserPoolProvider : IUserPoolProvider<User>
    {
        private readonly string clientId;
        private readonly CognitoUserPool cognitoUserPool;
        private readonly AmazonCognitoIdentityProviderClient cognitoIdentityProvider;
        private readonly IConfiguration configuration;

        public AwsCognitoUserPoolProvider(IConfiguration configuration)
        {
            string poolId = configuration["AWS:UserPool:PoolID"];
            this.clientId = configuration["AWS:UserPool:ClientID"];
            string clientSecret = configuration["AWS:UserPool:ClientSecret"];
            string identityPoolId = configuration["AWS:IdentityPool:PoolID"];
            RegionEndpoint regionEndpoint = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]);

            CognitoAWSCredentials cognitoAwsCredentials = new (identityPoolId, regionEndpoint);

            this.cognitoIdentityProvider = new AmazonCognitoIdentityProviderClient(
                cognitoAwsCredentials,
                regionEndpoint);

            this.cognitoUserPool = new CognitoUserPool(
                poolId,
                this.clientId,
                this.cognitoIdentityProvider,
                clientSecret);

            this.configuration = configuration;
        }

        public async Task<string> SignUp(
            string userName,
            string password,
            IDictionary<string, string> attributes,
            IDictionary<string, string>? validationData)
        {
            var response = await this.cognitoIdentityProvider.SignUpAsync(
                this.CreateSignUpRequest(
                    userName,
                    password,
                    attributes,
                    validationData));

            return response.UserSub;
        }

        public async Task ConfirmSignUp(
            string userName,
            string confirmationCode)
        {
            await this.cognitoUserPool.GetUser(userName)
                .ConfirmSignUpAsync(confirmationCode, false);
        }

        public async Task<User> SignIn(
            string userName,
            string password)
        {
            var cognitoUser = this.cognitoUserPool.GetUser(userName);
            await cognitoUser.StartWithSrpAuthAsync(new InitiateSrpAuthRequest { Password = password });

            return new User(
                cognitoUser.SessionTokens.AccessToken,
                cognitoUser.SessionTokens.IdToken,
                cognitoUser.SessionTokens.RefreshToken);
        }

        public Task<User> SignInGuest()
        {
            return this.SignIn(
                this.configuration["AWS:UserPool:GuestAccount:UserName"],
                this.configuration["AWS:UserPool:GuestAccount:Password"]);
        }

        public async Task<User> RefreshTokens(
            string refreshToken)
        {
            var authResponse = await this.cognitoIdentityProvider.InitiateAuthAsync(new InitiateAuthRequest
            {
                ClientId = this.clientId,
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                AuthParameters = new Dictionary<string, string>(StringComparer.Ordinal)
                    { { "REFRESH_TOKEN", refreshToken } },
            });

            return new User(
                authResponse.AuthenticationResult.AccessToken,
                authResponse.AuthenticationResult.IdToken,
                refreshToken);
        }

        public Task SignOut(string userName)
        {
            this.cognitoUserPool.GetUser(userName).SignOut();
            return Task.CompletedTask;
        }

        public Task PasswordReset(string userName)
        {
            return this.cognitoUserPool.GetUser(userName).ForgotPasswordAsync();
        }

        public async Task ConfirmPasswordReset(
            string userName,
            string confirmationCode,
            string newPassword)
        {
            await this.cognitoUserPool.GetUser(userName)
                .ConfirmForgotPasswordAsync(confirmationCode, newPassword);
        }

        private static List<AttributeType> CreateAttributeList(
            IDictionary<string, string> attributeDict)
        {
            return attributeDict.Select(pair => new AttributeType { Name = pair.Key, Value = pair.Value }).ToList();
        }

        private SignUpRequest CreateSignUpRequest(
            string userId,
            string password,
            IDictionary<string, string>? userAttributes,
            IDictionary<string, string>? validationData)
        {
            var attributeTypeList1 = userAttributes != null
                ? CreateAttributeList(userAttributes)
                : throw new ArgumentNullException(nameof(userAttributes), "userAttributes cannot be null.");
            var attributeTypeList2 = validationData != null ? CreateAttributeList(validationData) : null;
            return new SignUpRequest
            {
                Username = userId,
                Password = password,
                ClientId = this.clientId,
                UserAttributes = attributeTypeList1,
                ValidationData = attributeTypeList2,
            };
        }
    }
}