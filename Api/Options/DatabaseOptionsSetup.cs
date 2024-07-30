using Microsoft.Extensions.Options;

namespace Api.Options;

public class DatabaseOptionsSetup(IConfiguration configuration) : IConfigureOptions<DatabaseOptions>
{
    private const string ConfigurationSectionName = "DatabaseOptions";
    
    public void Configure(DatabaseOptions options)
    {
        // Get the connection string from the configuration
        var connectionStrings = configuration.GetConnectionString("DefaultConnection");

        // Set the connection string
        options.ConnectionString = connectionStrings!;
        
        // Bind the configuration to the options
        configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}