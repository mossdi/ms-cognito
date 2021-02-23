namespace AWS.Cognito.Net.Models
{
    public class User
    {
        public string AccessKey { get; init; }
        
        public string SecretKey { get; init; }
        
        public string SecurityToken { get; init; }
        
        public string RefreshToken { get; init; }
    }
}