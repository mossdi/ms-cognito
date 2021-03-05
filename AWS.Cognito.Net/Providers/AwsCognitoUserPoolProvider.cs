using Amazon;
using Amazon.Runtime;
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
        private readonly AmazonCognitoIdentityProviderClient _cognitoIdentityProvider;
        
        public AwsCognitoUserPoolProvider(IConfiguration configuration)
        {
            _poolId = configuration["AWS:UserPool:PoolID"];
            _clientId = configuration["AWS:UserPool:ClientID"];
            _clientSecret = configuration["AWS:UserPool:ClientSecret"];
            _identityPoolId = configuration["AWS:IdentityPool:PoolID"];
            _regionEndpoint = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]); 
            
            var credentials = new CognitoAWSCredentials(_identityPoolId, _regionEndpoint);

            _cognitoIdentityProvider = new AmazonCognitoIdentityProviderClient(
                credentials, 
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
            var credentials = await GetUserCredentials(cognitoUser);
            
            return new User
            {
                AccessKey = credentials.AccessKey,
                SecretKey = credentials.SecretKey,
                SecurityToken = credentials.Token,
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

            var authIssuedTime = DateTime.Now;
            var authExpirationTime = authIssuedTime.AddSeconds(authResponse.AuthenticationResult.ExpiresIn); 
            
            var cognitoUser = _cognitoUserPool.GetUser();
            
            cognitoUser.SessionTokens = new CognitoUserSession(
                authResponse.AuthenticationResult.IdToken,
                authResponse.AuthenticationResult.AccessToken,
                refreshToken,
                authIssuedTime,
                authExpirationTime);
            
            var credentials = await GetUserCredentials(cognitoUser);
            
            return new User
            {
                AccessKey = credentials.AccessKey,
                SecretKey = credentials.SecretKey,
                SecurityToken = credentials.Token,
                RefreshToken = cognitoUser.SessionTokens.RefreshToken,
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

        private async Task<ImmutableCredentials> GetUserCredentials(CognitoUser user)
        {
            return await user
                .GetCognitoAWSCredentials(_identityPoolId, _regionEndpoint)
                .GetCredentialsAsync();
        }
    }
}