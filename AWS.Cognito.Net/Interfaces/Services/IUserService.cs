// <copyright file="IUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Interfaces.Services
{
    using System.Threading.Tasks;
    using AWS.Cognito.Net.Models;

    public interface IUserService<TUser>
    {
        Task<string> SignUp(SignUpForm form);

        Task ConfirmSignUp(ConfirmSignUpForm form);

        Task<TUser> SignIn(LoginForm form);

        Task<TUser> SignInGuest();

        Task<TUser> RefreshTokens(RefreshTokensForm form);

        Task SignOut(SignOutForm form);

        Task PasswordReset(PasswordResetForm form);

        Task ConfirmPasswordReset(ConfirmPasswordResetForm form);
    }
}