using Npgsql;
using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. DATABASE CONFIGURATION
// Pull variables from Azure Environment Variables (Settings > Environment variables)
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPass = Environment.GetEnvironmentVariable("DB_PASS");

var connectionStringBuilder = new NpgsqlConnectionStringBuilder
{
    Host = dbHost,
    Database = dbName,
    Username = dbUser,
    Password = dbPass,
    Port = 5432, 
    SslMode = SslMode.Require,
    TrustServerCertificate = true // Often needed for cloud DB providers like Neon
};

// 2. SERVICE REGISTRATION
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionStringBuilder.ConnectionString));

builder.Services.AddControllers();

// 3. SWAGGER SETUP (The "Visual Skin" for your API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Use the absolute full name so there is no confusion
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Siemens Login API", 
        Version = "v1" 
    });
});

// 4. CORS CONFIGURATION
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        // During development/demo, it's safer to allow any origin 
        // to avoid "CORS blocked" errors in your interview.
        policy.AllowAnyOrigin() 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 5. AUTO-MIGRATION LOGIC (Runs every time the container starts)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); 
        Console.WriteLine("Database migration successful!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during migration: {ex.Message}");
    }
}

// 6. MIDDLEWARE PIPELINE
// We enable Swagger for both Dev and Production so you can show it in the interview
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Siemens API V1");
    // This makes Swagger available at the root (/) or /swagger
    c.RoutePrefix = "swagger"; 
});

// Important: UseCors must come before MapControllers
app.UseCors("AllowAll");

app.MapControllers();

// Ensure the app listens on the port Azure expects (8080)
app.Run();