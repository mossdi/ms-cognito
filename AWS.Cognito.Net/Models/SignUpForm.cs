// <copyright file="SignUpForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SignUpForm
    {
        public SignUpForm(string userName, string password, string email)
        {
            this.UserName = userName;
            this.Password = password;
            this.Email = email;
        }

        [Required]
        public string UserName { get; }

        [Required]
        public string Password { get; }

        public string Email { get; }
    }
}
