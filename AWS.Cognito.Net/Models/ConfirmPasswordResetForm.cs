// <copyright file="ConfirmPasswordResetForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ConfirmPasswordResetForm
    {
        public ConfirmPasswordResetForm(string userName, string confirmationCode, string newPassword)
        {
            this.UserName = userName;
            this.ConfirmationCode = confirmationCode;
            this.NewPassword = newPassword;
        }

        [Required]
        public string UserName { get; }

        [Required]
        public string ConfirmationCode { get; }

        [Required]
        public string NewPassword { get; }
    }
}