using Microsoft.EntityFrameworkCore;

namespace PlainML;

public class PlainMLContext : DbContext
{
    public PlainMLContext(DbContextOptions<PlainMLContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlainMLContext).Assembly);
    }
}
