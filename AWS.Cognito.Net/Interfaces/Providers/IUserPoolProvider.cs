// <copyright file="IUserPoolProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Interfaces.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserPoolProvider<TUser>
    {
        Task<string> SignUp(
            string userId,
            string password,
            IDictionary<string, string> attributes,
            IDictionary<string, string>? validationData);

        Task ConfirmSignUp(
            string userId,
            string confirmationCode);

        Task<TUser> SignIn(
            string userId,
            string password);

        Task<TUser> SignInGuest();

        Task<TUser> RefreshTokens(
            string refreshToken);

        Task SignOut(string userId);

        Task PasswordReset(string userName);

        Task ConfirmPasswordReset(
            string userName,
            string confirmationCode,
            string newPassword);
    }
}
