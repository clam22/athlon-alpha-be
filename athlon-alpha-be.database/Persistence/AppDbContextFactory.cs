using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace athlon_alpha_be.database.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var root = Path.Combine(Directory.GetCurrentDirectory(), "..");
    var envPath = Path.Combine(root, ".env");
    if (File.Exists(envPath))
    {
        DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { envPath }));
    }

    var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DatabaseConnection");

    
    if (string.IsNullOrEmpty(connectionString))
    {
        var apiPath = Path.Combine(root, "athlon-alpha-be.api");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        connectionString = configuration.GetConnectionString("DatabaseConnection");
    }

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException(
            "Connection string not found. Ensure 'ConnectionStrings__DatabaseConnection' is set in .env " +
            "or 'ConnectionStrings:DatabaseConnection' is set in configuration."
        );
    }

    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseNpgsql(connectionString)
        .Options;

    return new AppDbContext(options);
    }

}
