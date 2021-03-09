// <copyright file="PasswordResetForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PasswordResetForm
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Required]
        public string UserName { get; set; } = null!;
    }
}
