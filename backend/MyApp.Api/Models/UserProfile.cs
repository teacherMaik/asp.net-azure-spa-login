namespace MyApp.Api.Models 
{
    public class UserProfile
    {
        public string? Id { get; set; }
        public string? Provider { get; set; }
        public string? Email { get; set; }
        public string? AppId { get; set; }
        public string? DisplayName { get; set;}
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }    
}