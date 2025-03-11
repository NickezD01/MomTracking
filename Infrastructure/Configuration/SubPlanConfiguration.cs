using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class SubplanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            //SubPlan-Sub relationship
            builder.HasMany(s => s.Subscriptions)
                 .WithOne(s => s.SubscriptionPlans)
                 .HasForeignKey(s => s.PlanId)
                 .OnDelete(DeleteBehavior.Restrict);

            //SubPlan-Order relationship
            builder.HasMany(o => o.Orders)
                .WithOne(o => o.SubscriptionPlans)
                .HasForeignKey(o => o.PlanId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
