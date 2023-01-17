using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlainML.Entities;

namespace PlainML.Configurations;

internal class DeploymenttargetConfiguration : IEntityTypeConfiguration<Deploymenttarget>
{
    public void Configure(EntityTypeBuilder<Deploymenttarget> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(255);
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
