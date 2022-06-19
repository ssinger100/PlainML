using Microsoft.EntityFrameworkCore;
using ML_Trackingstore.Entities;

namespace ML_Trackingstore;

public class MLTrackingstoreContext : DbContext
{
    public DbSet<MLModel> MLModels { get; set; } = null!;

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
