namespace AWS.Cognito.Net.Models
{
    public class User
    {
        public string AccessToken { get; init; }
        
        public string IdentityToken { get; init; }
        
        public string RefreshToken { get; init; }
    }
}