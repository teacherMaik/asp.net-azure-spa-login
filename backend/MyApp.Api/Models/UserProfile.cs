namespace MyApp.Api.Models 
{
    public class UserProfile
    {
        // Unique key. Postgres handles the +1 automatically.
        public int Id { get; set; } 

        // The ID from the provider (e.g., Google/Microsoft)
        public string? ExternalId { get; set; } 
        
        public string? Provider { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set;}
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }    
}