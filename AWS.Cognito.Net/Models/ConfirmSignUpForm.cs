using System.ComponentModel.DataAnnotations;

namespace AWS.Cognito.Net.Models
{
    public class ConfirmSignUpForm
    {
        [Required]
        public string UserName { get; set; }

        [Required] 
        public string ConfirmationCode { get; set; }
    }
}