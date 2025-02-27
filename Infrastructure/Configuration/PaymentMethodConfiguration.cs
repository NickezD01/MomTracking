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
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            //PaymentMethod-Payment relationship
            builder.HasMany(p => p.Payments)
                 .WithOne(p => p.PaymentMethod)
                 .HasForeignKey(p => p.MethodId)
                 .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
