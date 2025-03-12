// Infrastructure/Configuration/OrderConfiguration.cs
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Infrastructure.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Order-Payment relationship
            builder.HasMany(p => p.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Thêm mối quan hệ Order-Subscription
            builder.HasOne(o => o.Subscription)
                .WithMany()
                .HasForeignKey(o => o.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}