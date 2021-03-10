// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    public class User
    {
        public User(string accessToken, string identityToken, string refreshToken)
        {
            this.AccessToken = accessToken;
            this.IdentityToken = identityToken;
            this.RefreshToken = refreshToken;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string AccessToken { get; }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string IdentityToken { get; }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string RefreshToken { get; }
    }
}