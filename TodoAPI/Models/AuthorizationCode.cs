namespace TodoAPI.Models
{
    public class AuthorizationCode
    {
        public string Code { get; set; }
        public string ClientSecret { get; set; }
    }
}