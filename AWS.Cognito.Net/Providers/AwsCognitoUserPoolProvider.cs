using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using AWS.Cognito.Net.Interfaces.Providers;
using AWS.Cognito.Net.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.Cognito.Net.Providers
{
    public class AwsCognitoUserPoolProvider<TUser>: IUserPoolProvider<User>
    {
        private readonly CognitoUserPool _cognitoUserPool;
        
        public AwsCognitoUserPoolProvider(IConfiguration configuration)
        {
            var credentials = new CognitoAWSCredentials(
                configuration["AWS:IdentityPool:AccountId"],
                configuration["AWS:IdentityPool:PoolID"], 
                configuration["AWS:IdentityPool:UnAuthRoleARN"],
                configuration["AWS:IdentityPool:AuthRoleARN"],
                RegionEndpoint.GetBySystemName(configuration["AWS:Region"]));
            
            var amazonCognitoIdentityProviderClient = new AmazonCognitoIdentityProviderClient(
                credentials,
                RegionEndpoint.GetBySystemName(configuration["AWS:Region"]));

            _cognitoUserPool = new CognitoUserPool(
                configuration["AWS:UserPool:PoolID"],
                configuration["AWS:UserPool:ClientID"], 
                amazonCognitoIdentityProviderClient,
                configuration["AWS:UserPool:ClientSecret"]);
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
            
            var userDetails = await cognitoUser.GetUserDetailsAsync();
            
            return new User // TODO: Change it to AutoMapping
            {
                UserName = userDetails.Username,
                Email = userDetails.UserAttributes.Find(attribute => attribute.Name.Equals("email"))?.Value,
                AccessToken = cognitoUser.SessionTokens.AccessToken,
                IdentityToken = cognitoUser.SessionTokens.IdToken,
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