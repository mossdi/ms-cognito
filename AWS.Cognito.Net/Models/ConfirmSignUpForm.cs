// <copyright file="ConfirmSignUpForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ConfirmSignUpForm
    {
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        [Required]
        public string UserName { get; set; } = null!;

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        [Required]
        public string ConfirmationCode { get; set; } = null!;
    }
}