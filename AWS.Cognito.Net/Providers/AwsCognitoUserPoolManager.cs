using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using AWS.Cognito.Net.Interfaces.Providers;
using AWS.Cognito.Net.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.CognitoIdentity;

namespace AWS.Cognito.Net.Providers
{
    public class AwsCognitoUserPoolManager<TUser>: IUserPoolProvider<User>
    {
        private readonly CognitoUserPool _cognitoUserPool;
        
        public AwsCognitoUserPoolManager(IConfiguration configuration)
        {
            var credentials = new CognitoAWSCredentials(
                configuration["AWS:IdentityPool:AccountId"],
                configuration["AWS:IdentityPool:PoolID"], 
                configuration["AWS:IdentityPool:UnauthRoleARN"],
                configuration["AWS:IdentityPool:AuthRoleARN"],
                RegionEndpoint.GetBySystemName(configuration["AWS:Region"]) 
            );
            
            var amazonCognitoIdentityProviderClient = new AmazonCognitoIdentityProviderClient(credentials);

            _cognitoUserPool = new CognitoUserPool(
                configuration["AWS:UserPool:PoolID"],
                configuration["AWS:UserPool:ClientID"], 
                amazonCognitoIdentityProviderClient,
                configuration["AWS:UserPool:ClientSecret"]);
        }
        
        public async Task<User> SignUp(
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
            
            var cognitoUser = await _cognitoUserPool.FindByIdAsync(userName);

            return new User // TODO: Change it to AutoMapping
            {
                UserName = cognitoUser.Username,
                Email = cognitoUser.Attributes["email"]
            };
        }
        
        public async Task ConfirmSignUp(
            string userName,
            string confirmationCode)
        {
            var cognitoUser = await _cognitoUserPool.FindByIdAsync(userName);
            
            await cognitoUser.ConfirmSignUpAsync(
                confirmationCode,
                false);
        }
        
        public async Task<User> SignIn(            
            string userName, 
            string password)
        {
            var cognitoUser = await _cognitoUserPool.FindByIdAsync(userName);
            
            var authRequest = new InitiateSrpAuthRequest()
            {
                Password = password
            };
            
            await cognitoUser.StartWithSrpAuthAsync(authRequest);
            
            return new User // TODO: Change it to AutoMapping
            {
                UserName = cognitoUser.Username,
                Email = cognitoUser.Attributes["email"],
                Token = cognitoUser.SessionTokens.AccessToken
            };
        }

        public async Task<User> SignOut(string userName)
        {
            var cognitoUser = await _cognitoUserPool.FindByIdAsync(userName);
            
            cognitoUser.SignOut();
            
            return new User // TODO: Change it to AutoMapping
            {
                UserName = cognitoUser.Username,
                Email = cognitoUser.Attributes["email"]
            };
        }
    }
}