using System.ComponentModel.DataAnnotations;

namespace AWS.Cognito.Net.Models
{
    public class PasswordResetForm
    {
        [Required]
        public string UserName { get; set; }
    }
}
