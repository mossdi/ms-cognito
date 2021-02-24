using AWS.Cognito.Net.Models;
using AWS.Cognito.Net.Interfaces.Providers;
using AWS.Cognito.Net.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.Cognito.Net.Services
{
    public class UserService<TUser> : IUserService<User>
    {
        private readonly IUserPoolProvider<User> _userPoolProvider;

        public UserService(IUserPoolProvider<User> userPoolProvider)
        {
            _userPoolProvider = userPoolProvider;
        }
        
        public async Task SignUp(SignUpForm form)
        {
            var attributes = new Dictionary<string, string>();
            if (form.Email != null) attributes.Add("email", form.Email);

            await _userPoolProvider.SignUp(
                form.UserName,
                form.Password,
                attributes,
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

        public Task<User> RefreshTokens(RefreshTokensForm form)
        {
            return _userPoolProvider.RefreshTokens(
                form.RefreshToken);
        }

        public async Task SignOut(SignOutForm form)
        { 
            await _userPoolProvider.SignOut(form.UserName);
        }

        public async Task PasswordReset(PasswordResetForm form)
        {
            await _userPoolProvider.PasswordReset(form.UserName);
        }

        public async Task ConfirmPasswordReset(ConfirmPasswordResetForm form)
        {
            await _userPoolProvider.ConfirmPasswordReset(
                form.UserName,
                form.ConfirmationCode,
                form.NewPassword);
        }
    }
}
