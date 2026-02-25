using Microsoft.EntityFrameworkCore;
using MyApp.Api.Models;

namespace MyApp.Api.Data
{
    public class AppDbContext : DbContext
    {
        // This constructor is required for ASP.NET to "inject" the database connection string
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // This represents the "UserProfiles" table in our Neon Database
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicitly tell EF that 'Id' is the Primary Key
            modelBuilder.Entity<UserProfile>().HasKey(u => u.Id);

            // Configure 'Id' to be an identity column (auto-increment) in Postgres
            modelBuilder.Entity<UserProfile>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}