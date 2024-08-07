using System.Diagnostics;
using Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Data;

public class DataContext : DbContext
{
    private readonly DatabaseOptions _databaseOptions;

    public DataContext(DbContextOptions<DataContext> options, IOptions<DatabaseOptions> opt) : base(options)
    {
        _databaseOptions = opt.Value;
    }

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
                Debug.WriteLine(message), new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
        }
    }

    public DbSet<RpgCharacter> RpgCharacters => Set<RpgCharacter>();
}