﻿using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class ChildrenConfiguration : IEntityTypeConfiguration<Children>
    {
        public void Configure(EntityTypeBuilder<Children> builder)
        {
            //Childrent-HealthMetric relationship
            builder.HasMany(h => h.HealthMetrics)
                 .WithOne(h => h.Childrent)
                 .HasForeignKey(h => h.ChildrentId);



        }
    }
}
