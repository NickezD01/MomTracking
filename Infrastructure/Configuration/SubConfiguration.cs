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
    public class SubConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {

            //Order-Sub relationship
            builder.HasOne(o => o.Order)
                .WithOne(o => o.Subscription)
                .HasForeignKey<Order>(o => o.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
