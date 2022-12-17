using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ML_Trackingstore.Entities;

namespace ML_Trackingstore.Configurations;

internal class DeploymentConfiguration : IEntityTypeConfiguration<Deployment>
{
    public void Configure(EntityTypeBuilder<Deployment> builder)
    {
        builder.HasIndex(x => new { ExperimentId = x.Experiment.Id, DeploymenttargerId = x.Deploymenttarget.Id,RunId = x.Run.Id });
    }
}
