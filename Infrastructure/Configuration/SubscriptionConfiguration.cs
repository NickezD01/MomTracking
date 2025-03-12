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
            builder.HasMany(s => s.Orders)
                .WithOne(o => o.Subscription)
                .HasForeignKey(o => o.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}