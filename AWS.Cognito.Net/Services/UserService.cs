using AWS.Cognito.Net.Models;
using AWS.Cognito.Net.Interfaces.Providers;
using AWS.Cognito.Net.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.Cognito.Net.Services
{
    public class UserService<TUser>: IUserService<User>
    {
        private readonly IUserPoolProvider<User> _userPoolProvider;

        public UserService(IUserPoolProvider<User> userPoolProvider)
        {
            _userPoolProvider = userPoolProvider;
        }
        
        public async Task<User> SignUp(SignUpForm form)
        {
            return await _userPoolProvider.SignUp(
                form.UserName,
                form.Password,
                new Dictionary<string, string> {{"email", form.Email}},
                null);
        }

        public async Task ConfirmSignUp(ConfirmSignUpForm form)
        { 
            await _userPoolProvider.ConfirmSignUp(
                form.UserName,
                form.ConfirmationCode);
        }
        
        public async Task<User> SignIn(LoginForm form)
        { 
            return await _userPoolProvider.SignIn(
                form.UserName,
                form.Password);
        }

        public async Task<User> SignOut(SignOutForm form)
        {
            return await _userPoolProvider.SignOut(
                form.UserName);
        }
    }
}
