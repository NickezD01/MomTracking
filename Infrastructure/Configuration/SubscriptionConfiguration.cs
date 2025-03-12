// Infrastructure/Configuration/SubscriptionConfiguration.cs
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            // Subscription-Order relationship
            builder.HasOne(s => s.Order)
                .WithOne(o => o.Subscription)
                .HasForeignKey<Order>(o => o.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}