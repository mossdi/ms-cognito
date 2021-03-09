// <copyright file="LoginForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginForm
    {
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        [Required]
        public string UserName { get; set; } = null!;

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        [Required]
        public string Password { get; set; } = null!;
    }
}