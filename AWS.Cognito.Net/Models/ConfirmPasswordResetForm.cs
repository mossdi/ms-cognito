// <copyright file="ConfirmPasswordResetForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ConfirmPasswordResetForm
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Required]
        public string UserName { get; set; } = null!;

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Required]
        public string ConfirmationCode { get; set; } = null!;

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Required]
        public string NewPassword { get; set; } = null!;
    }
}