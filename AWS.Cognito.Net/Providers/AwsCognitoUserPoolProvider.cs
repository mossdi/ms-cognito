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
                Token = cognitoUser.SessionTokens.AccessToken
            };
        }

        public async Task SignOut(string userName)
        {
            var cognitoUser = await _cognitoUserPool.FindByIdAsync(userName);
            
            cognitoUser.SignOut();
        }

        public async Task PasswordReset(string userName)
        { 
            var cognitoUser = await _cognitoUserPool.FindByIdAsync(userName);

            await cognitoUser.ForgotPasswordAsync();
        }
    }
}