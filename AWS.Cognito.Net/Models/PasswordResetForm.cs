// <copyright file="PasswordResetForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PasswordResetForm
    {
        public PasswordResetForm(string userName)
        {
            this.UserName = userName;
        }

        [Required]
        public string UserName { get; }
    }
}
