using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class HealthMetricConfiguration
    {
        public void Configure(EntityTypeBuilder<HealthMetric> builder)
        {
            builder.HasOne(h => h.GrowthIndex)
                 .WithOne(h => h.HealthMetric)
                 .HasForeignKey<GrowthIndex>(h => h.HealthMetricId);
        }
    }
}
