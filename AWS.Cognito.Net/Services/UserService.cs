using AWS.Cognito.Net.Models;
using AWS.Cognito.Net.Interfaces.Providers;
using AWS.Cognito.Net.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.Cognito.Net.Services
{
    public class UserService<TUser>: IUserService<User>
    {
        private readonly IUserPoolProvider<User> _userPoolManager;

        public UserService(IUserPoolProvider<User> userPoolManager)
        {
            _userPoolManager = userPoolManager;
        }
        
        public async Task<User> SignUp(SignUpForm form)
        {
            return await _userPoolManager.SignUp(
                form.UserName,
                form.Password,
                new Dictionary<string, string> {{"email", form.Email}},
                null);
        }

        public async Task<bool> ConfirmSignUp(ConfirmSignUpForm form)
        {
            return await _userPoolManager.ConfirmSignUp(
                form.UserName,
                form.ConfirmationCode);
        }
        
        public async Task<User> SignIn(LoginForm form)
        { 
            return await _userPoolManager.SignIn(
                form.UserName,
                form.Password);
        }

        public async Task<User> SignOut(SignOutForm form)
        {
            return await _userPoolManager.SignOut(
                form.UserName);
        }
    }
}
