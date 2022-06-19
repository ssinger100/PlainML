using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ML_Trackingstore.Entities;

namespace ML_Trackingstore.Configurations;

internal class RunConfiguration : IEntityTypeConfiguration<Run>
{
    public void Configure(EntityTypeBuilder<Run> builder)
    {
        builder.HasMany(x => x.Parameters).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Parameter_StringType).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Metrics).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}
