using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.Cognito.Net.Interfaces.Providers
{
    public interface IUserPoolProvider<TUser>
    { 
        Task SignUp(
            string userId,
            string password,
            Dictionary<string, string> attributes,
            Dictionary<string, string> validationData);

        Task ConfirmSignUp(
            string userId,
            string confirmationCode);
        
        Task<TUser> SignIn(
            string userId,
            string password);

        Task SignOut(string userId);
        
        Task PasswordReset(string userName);
    }
}
