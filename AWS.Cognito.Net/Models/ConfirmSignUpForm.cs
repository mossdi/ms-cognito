// <copyright file="ConfirmSignUpForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ConfirmSignUpForm
    {
        public ConfirmSignUpForm(string userName, string confirmationCode)
        {
            this.UserName = userName;
            this.ConfirmationCode = confirmationCode;
        }

        [Required]
        public string UserName { get; }

        [Required]
        public string ConfirmationCode { get; }
    }
}