// Infrastructure/Configuration/SubPlanConfiguration.cs
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
    public class SubPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            //SubPlan-Sub relationship
            builder.HasMany(s => s.Subscriptions)
                .WithOne(s => s.SubscriptionPlans)
                .HasForeignKey(s => s.PlanId)
                .OnDelete(DeleteBehavior.Restrict);
             
            // Xóa bỏ mối quan hệ trực tiếp với Order
            // Không còn cần thiết vì Order sẽ liên kết với Subscription
        }
    }
}