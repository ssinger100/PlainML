using Microsoft.EntityFrameworkCore;
using PlainML.Entities;

namespace PlainML;

public class PlainMLContext : DbContext
{
    public DbSet<Experiment> MLModels { get; set; } = null!;

    public PlainMLContext()
    {

    }

    public PlainMLContext(DbContextOptions<PlainMLContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlainMLContext).Assembly);
    }
}
