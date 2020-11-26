namespace AWS.Cognito.Net.Models
{
    public class User
    {
        public string UserName { get; init; }
        
        public string Email { get; init; }
        
        public string AccessToken { get; init; }
        
        public string IdentityToken { get; init; }
    }
}