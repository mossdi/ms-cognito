using System.ComponentModel.DataAnnotations;

namespace AWS.Cognito.Net.Models
{
    public class ConfirmPasswordResetForm
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string ConfirmationCode { get; set; }
        
        [Required]
        public string NewPassword { get; set; }
    }
}