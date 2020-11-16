using AWS.Cognito.Net.Models;
using AWS.Cognito.Net.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AWS.Cognito.Net.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService<User> _userService;
        
        public UserController(IUserService<User> userService)
        {
            _userService = userService;
        }
        
        [HttpPost]
        [Route("sign-up")]
        public async Task<User> SignUp(SignUpForm form)
        {
            return await _userService.SignUp(form);
        }

        [HttpPost]
        [Route("confirm-sign-up")]
        public async Task ConfirmSignUp(ConfirmSignUpForm form)
        { 
            await _userService.ConfirmSignUp(form);
        }
        
        [HttpPost]
        [Route("sign-in")]
        public async Task<User> SignIn(LoginForm form)
        {
            return await _userService.SignIn(form);
        }

        [HttpPost]
        [Route("sign-out")]
        public async Task<User> SignOut(SignOutForm form)
        {
            return await _userService.SignOut(form);
        }
    }
}
