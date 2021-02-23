using System.ComponentModel.DataAnnotations;

namespace AWS.Cognito.Net.Models
{
    public class RefreshTokensForm
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}