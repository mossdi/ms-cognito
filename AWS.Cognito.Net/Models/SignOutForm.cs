using System.ComponentModel.DataAnnotations;

namespace AWS.Cognito.Net.Models
{
    public class SignOutForm
    {
        [Required]
        public string UserName { get; set; }
    }
}