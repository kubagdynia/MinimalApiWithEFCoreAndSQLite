using System.Diagnostics;
using Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Data;

public class DataContext(DbContextOptions<DataContext> options, IOptions<DatabaseOptions> opt) : DbContext(options)
{
    private readonly DatabaseOptions _databaseOptions = opt.Value;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure the database options
        optionsBuilder.UseSqlite(_databaseOptions.ConnectionString, optBuilder =>
            {
                optBuilder.CommandTimeout(_databaseOptions.CommandTimeout);
            })
            .EnableDetailedErrors(_databaseOptions.EnableDetailedErrors)
            .EnableSensitiveDataLogging(_databaseOptions.EnableSensitiveDataLogging);
        
        if (_databaseOptions.LogToDebugWriteLine)
        {
            optionsBuilder.LogTo(message =>
                Debug.WriteLine(message), [DbLoggerCategory.Database.Command.Name], LogLevel.Information);
        }
    }

    public DbSet<RpgCharacter> RpgCharacters => Set<RpgCharacter>();
}