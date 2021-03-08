// <copyright file="LoginForm.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AWS.Cognito.Net.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginForm
    {
        public LoginForm(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }

        [Required]
        public string UserName { get; }

        [Required]
        public string Password { get; }
    }
}