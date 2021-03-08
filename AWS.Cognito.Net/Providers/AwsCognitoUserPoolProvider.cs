using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.Extensions.Configuration;
using AWS.Cognito.Net.Interfaces.Providers;
using AWS.Cognito.Net.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace AWS.Cognito.Net.Providers
{
    public class AwsCognitoUserPoolProvider<TUser> : IUserPoolProvider<User>
    {
        private readonly string _poolId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _identityPoolId;
        private readonly RegionEndpoint _regionEndpoint;
        private readonly CognitoUserPool _cognitoUserPool;
        private readonly CognitoAWSCredentials _cognitoAwsCredentials;
        private readonly AmazonCognitoIdentityProviderClient _cognitoIdentityProvider;
        
        public AwsCognitoUserPoolProvider(IConfiguration configuration)
        {
            _poolId = configuration["AWS:UserPool:PoolID"];
            _clientId = configuration["AWS:UserPool:ClientID"];
            _clientSecret = configuration["AWS:UserPool:ClientSecret"];
            _identityPoolId = configuration["AWS:IdentityPool:PoolID"];
            _regionEndpoint = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]); 
            
            _cognitoAwsCredentials = new CognitoAWSCredentials(
                _identityPoolId, 
                _regionEndpoint);

            _cognitoIdentityProvider = new AmazonCognitoIdentityProviderClient(
                _cognitoAwsCredentials, 
                _regionEndpoint);
            
            _cognitoUserPool = new CognitoUserPool(
                _poolId,
                _clientId, 
                _cognitoIdentityProvider,
                _clientSecret);
        }
        
        public async Task SignUp(
            string userName, 
            string password, 
            Dictionary<string, string> attributes, 
            Dictionary<string, string> validationData)
        {
            await _cognitoUserPool.SignUpAsync(
                userName,
                password,
                attributes,
                validationData);
        }
        
        public async Task ConfirmSignUp(
            string userName,
            string confirmationCode)
        {
            await _cognitoUserPool.GetUser(userName)
                .ConfirmSignUpAsync(confirmationCode, false);
        }
        
        public async Task<User> SignIn(            
            string userName, 
            string password)
        {
            var cognitoUser = _cognitoUserPool.GetUser(userName);
            await cognitoUser.StartWithSrpAuthAsync(new InitiateSrpAuthRequest { Password = password });
            
            return new User
            {
                AccessToken = cognitoUser.SessionTokens.AccessToken,
                IdentityToken = cognitoUser.SessionTokens.IdToken,
                RefreshToken = cognitoUser.SessionTokens.RefreshToken,
            };
        }

        public async Task<User> SignInGuest()
        {
            throw new NotImplementedException();
        }

        public async Task<User> RefreshTokens(
            string refreshToken)
        {
            var authResponse = await _cognitoIdentityProvider.InitiateAuthAsync(new InitiateAuthRequest
            {
                ClientId = _clientId,
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                AuthParameters = new Dictionary<string, string> {{"REFRESH_TOKEN", refreshToken}}
            });
            
            return new User
            {
                AccessToken = authResponse.AuthenticationResult.AccessToken,
                IdentityToken = authResponse.AuthenticationResult.IdToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task SignOut(string userName)
        { 
            _cognitoUserPool.GetUser(userName).SignOut();
        }

        public async Task PasswordReset(string userName)
        { 
            await _cognitoUserPool.GetUser(userName).ForgotPasswordAsync();
        }
        
        public async Task ConfirmPasswordReset(
            string userName,
            string confirmationCode,
            string newPassword)
        { 
            await _cognitoUserPool.GetUser(userName)
                .ConfirmForgotPasswordAsync(confirmationCode, newPassword);
        }
    }
}