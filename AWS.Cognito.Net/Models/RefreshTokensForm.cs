// <copyright file="RefreshTokensForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RefreshTokensForm
    {
        public RefreshTokensForm(string refreshToken)
        {
            this.RefreshToken = refreshToken;
        }

        [Required]
        public string RefreshToken { get; }
    }
}