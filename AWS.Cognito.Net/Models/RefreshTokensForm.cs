// <copyright file="RefreshTokensForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RefreshTokensForm
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}