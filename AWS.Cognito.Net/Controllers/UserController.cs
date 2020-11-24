using AWS.Cognito.Net.Models;
using AWS.Cognito.Net.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AWS.Cognito.Net.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService<User> _userService;
        
        public UserController(IUserService<User> userService)
        {
            _userService = userService;
        }
        
        [HttpPost]
        [Route("[action]")]
        public async Task SignUp(SignUpForm form)
        { 
            await _userService.SignUp(form);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task ConfirmSignUp(ConfirmSignUpForm form)
        { 
            await _userService.ConfirmSignUp(form);
        }
        
        [HttpPost]
        [Route("[action]")]
        public async Task<User> SignIn(LoginForm form)
        {
            return await _userService.SignIn(form);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task SignOut(SignOutForm form)
        { 
            await _userService.SignOut(form);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task PasswordReset(PasswordResetForm form)
        { 
            await _userService.PasswordReset(form);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task ConfirmPasswordReset(ConfirmPasswordResetForm form)
        {
            await _userService.ConfirmPasswordReset(form);
        }
    }
}
