// <copyright file="SignUpForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SignUpForm
    {
        [Required]

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string UserName { get; set; } = null!;

        [Required]

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Password { get; set; } = null!;

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string? Email { get; set; }
    }
}
