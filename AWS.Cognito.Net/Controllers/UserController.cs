// <copyright file="UserController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Controllers
{
    using System.Threading.Tasks;
    using AWS.Cognito.Net.Interfaces.Services;
    using AWS.Cognito.Net.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService<User> userService;

        public UserController(IUserService<User> userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task SignUp(SignUpForm form)
        {
            await this.userService.SignUp(form);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task ConfirmSignUp(ConfirmSignUpForm form)
        {
            await this.userService.ConfirmSignUp(form);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<User> SignIn(LoginForm form)
        {
            return await this.userService.SignIn(form);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<User> SignInGuest(LoginGuestForm form)
        {
            return await this.userService.SignInGuest(form);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<User> RefreshTokens(RefreshTokensForm form)
        {
            return await this.userService.RefreshTokens(form);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task SignOut(SignOutForm form)
        {
            await this.userService.SignOut(form);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task PasswordReset(PasswordResetForm form)
        {
            await this.userService.PasswordReset(form);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task ConfirmPasswordReset(ConfirmPasswordResetForm form)
        {
            await this.userService.ConfirmPasswordReset(form);
        }
    }
}
