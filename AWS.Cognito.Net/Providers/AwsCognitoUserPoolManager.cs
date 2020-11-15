using Amazon;
using Amazon.Runtime;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using AWS.Cognito.Net.Interfaces.Providers;
using AWS.Cognito.Net.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.Cognito.Net.Providers
{
    public class AwsCognitoUserPoolManager<TUser>: IUserPoolProvider<User>
    {
        private readonly CognitoUserPool _cognitoUserPool;
        
        public AwsCognitoUserPoolManager(IConfiguration configuration)
        {
            var basicAwsCredentials = new BasicAWSCredentials(
                configuration["AWS:Credentials:AccessKey"],
                configuration["AWS:Credentials:SecretKey"]);
            
            var amazonCognitoIdentityProviderClient = new AmazonCognitoIdentityProviderClient(
                basicAwsCredentials, 
                RegionEndpoint.GetBySystemName(configuration["AWS:Region"]));

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
            _cognitoUserPool.SignUpAsync(
                userName,
                password,
                attributes,
                validationData).Wait();
            
            var cognitoUser = await _cognitoUserPool.FindByIdAsync(userName);

            return new User // TODO: Change it to AutoMapping
            {
                UserName = cognitoUser.Username,
                Email = cognitoUser.Attributes["email"]
            };
        }
        
        public async Task<bool> ConfirmSignUp(
            string userName,
            string confirmationCode)
        {
            var cognitoUser = await _cognitoUserPool.FindByIdAsync(userName);
            
            var confirmSignUpRequest = cognitoUser.ConfirmSignUpAsync(
                confirmationCode,
                false);

            confirmSignUpRequest.Wait();

            return confirmSignUpRequest.IsCompleted;
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
            
            cognitoUser.StartWithSrpAuthAsync(authRequest).Wait();
            
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