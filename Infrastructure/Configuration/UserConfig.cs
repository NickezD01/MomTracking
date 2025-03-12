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
    public class UserConfig : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.HasMany(s => s.Subscriptions)
                            .WithOne(s => s.Account)
                            .HasForeignKey(s => s.AccountId)
                            .OnDelete(DeleteBehavior.Restrict);

            //account-payment relationship
            builder.HasMany(p => p.Payments)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            //account-childrent relationship
            builder.HasMany(c => c.Childrents)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            //account-schedule relationship
            builder.HasMany(sc => sc.Schedules)
                .WithOne(sc => sc.Account)
                .HasForeignKey(sc => sc.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            //account-post relationship
            builder.HasMany(ps => ps.Posts)
                .WithOne(ps => ps.Account)
                .HasForeignKey(ps => ps.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            //account-notification relationship
            builder.HasMany(n => n.Notifications)
                 .WithOne(n => n.Account)
                 .HasForeignKey(n => n.AccountId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(h => h.Comments)
                .WithOne(h => h.Account)
                .HasForeignKey(h => h.AccountId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(x => x.Orders)
                .WithOne(x => x.UserAccount)
                .HasForeignKey(x => x.AccountId);
        }
    }
}
