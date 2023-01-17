using Microsoft.EntityFrameworkCore;
using PlainML.Entities;

namespace PlainML;

public class MLTrackingstoreContext : DbContext
{
    public DbSet<Experiment> MLModels { get; set; } = null!;

    public MLTrackingstoreContext()
    {

    }

    public MLTrackingstoreContext(DbContextOptions<MLTrackingstoreContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MLTrackingstoreContext).Assembly);
    }
}
