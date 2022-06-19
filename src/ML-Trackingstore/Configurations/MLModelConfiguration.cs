using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ML_Trackingstore.Entities;

namespace ML_Trackingstore.Configurations
{
    internal class MLModelConfiguration : IEntityTypeConfiguration<MLModel>
    {
        public void Configure(EntityTypeBuilder<MLModel> builder)
        {
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasMany(x => x.Runs).WithOne(x => x.MLModel).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
