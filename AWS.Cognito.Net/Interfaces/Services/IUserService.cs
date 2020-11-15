using System.Threading.Tasks;
using AWS.Cognito.Net.Models;

namespace AWS.Cognito.Net.Interfaces.Services
{
    public interface IUserService<TUser>
    {
        Task<TUser> SignUp(SignUpForm form);

        Task<bool> ConfirmSignUp(ConfirmSignUpForm form);
        
        Task<TUser> SignIn(LoginForm form);

        Task<TUser> SignOut(SignOutForm form);
    }
}