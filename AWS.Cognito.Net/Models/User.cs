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

        public string AccessToken { get; }

        public string IdentityToken { get; }

        public string RefreshToken { get; }
    }
}