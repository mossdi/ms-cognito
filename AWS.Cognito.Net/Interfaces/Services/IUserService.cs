using System.Threading.Tasks;
using AWS.Cognito.Net.Models;

namespace AWS.Cognito.Net.Interfaces.Services
{
    public interface IUserService<TUser>
    {
        Task SignUp(SignUpForm form);

        Task ConfirmSignUp(ConfirmSignUpForm form);
        
        Task<TUser> SignIn(LoginForm form);
        
        Task<TUser> SignInGuest(LoginGuestForm form);
        
        Task<TUser> RefreshTokens(RefreshTokensForm form);

        Task SignOut(SignOutForm form);

        Task PasswordReset(PasswordResetForm form);

        Task ConfirmPasswordReset(ConfirmPasswordResetForm form);
    }
}