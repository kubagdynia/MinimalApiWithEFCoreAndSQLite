using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<RpgCharacter> RpgCharacters => Set<RpgCharacter>();
}