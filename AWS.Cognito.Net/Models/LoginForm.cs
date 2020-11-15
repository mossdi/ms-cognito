using System.ComponentModel.DataAnnotations;

namespace AWS.Cognito.Net.Models
{
    public class LoginForm
    {
        [Required]
        public string UserName { get; set; }

        [Required] 
        public string Password { get; set; }
    }
}