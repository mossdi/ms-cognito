// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AWS.Cognito.Net.Interfaces.Providers;
    using AWS.Cognito.Net.Interfaces.Services;
    using AWS.Cognito.Net.Models;

    public class UserService : IUserService<User>
    {
        private readonly IUserPoolProvider<User> userPoolProvider;

        public UserService(IUserPoolProvider<User> userPoolProvider)
        {
            this.userPoolProvider = userPoolProvider;
        }

        public async Task<string> SignUp(SignUpForm form)
        {
            var attributes = new Dictionary<string, string>(System.StringComparer.Ordinal);

            if (form.Email is not null)
            {
                attributes["email"] = form.Email;
            }

            form.Email ??= form.UserName;

            return await this.userPoolProvider.SignUp(
                form.UserName,
                form.Password,
                attributes,
                null);
        }

        public async Task ConfirmSignUp(ConfirmSignUpForm form)
        {
            await this.userPoolProvider.ConfirmSignUp(
                form.UserName,
                form.ConfirmationCode);
        }

        public async Task<User> SignIn(LoginForm form)
        {
            return await this.userPoolProvider.SignIn(
                form.UserName,
                form.Password);
        }

        public async Task<User> SignInGuest(LoginGuestForm form)
        {
            return await this.userPoolProvider.SignInGuest();
        }

        public Task<User> RefreshTokens(RefreshTokensForm form)
        {
            return this.userPoolProvider.RefreshTokens(
                form.RefreshToken);
        }

        public async Task SignOut(SignOutForm form)
        {
            await this.userPoolProvider.SignOut(form.UserName);
        }

        public async Task PasswordReset(PasswordResetForm form)
        {
            await this.userPoolProvider.PasswordReset(form.UserName);
        }

        public async Task ConfirmPasswordReset(ConfirmPasswordResetForm form)
        {
            await this.userPoolProvider.ConfirmPasswordReset(
                form.UserName,
                form.ConfirmationCode,
                form.NewPassword);
        }
    }
}