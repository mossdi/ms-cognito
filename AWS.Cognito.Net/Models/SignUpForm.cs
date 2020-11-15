using System.ComponentModel.DataAnnotations;

namespace AWS.Cognito.Net.Models
{
    public class SignUpForm
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Email { get; set; }
    }
}
