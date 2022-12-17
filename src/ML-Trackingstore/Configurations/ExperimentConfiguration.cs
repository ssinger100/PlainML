﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ML_Trackingstore.Entities;

namespace ML_Trackingstore.Configurations
{
    internal class ExperimentConfiguration : IEntityTypeConfiguration<Experiment>
    {
        public void Configure(EntityTypeBuilder<Experiment> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(255);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasMany(x => x.Runs).WithOne(x => x.MLModel).OnDelete(DeleteBehavior.Cascade);
        }
    }
}