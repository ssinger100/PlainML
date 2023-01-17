using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlainML.Entities;

namespace PlainML.Configurations;

internal class DeploymentConfiguration : IEntityTypeConfiguration<Deployment>
{
    public void Configure(EntityTypeBuilder<Deployment> builder)
    {
        builder.HasIndex(x => new { x.ExperimentId, DeploymenttargerId = x.DeploymenttargetId, x.RunId });
    }
}
