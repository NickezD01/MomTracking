﻿using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Host=172.17.0.2; Port=5431; Database=koidelivery; Username=postgres; Password=matkhau;Include Error Detail=True;TrustServerCertificate=True");
        }
        public DbSet<UserAccount> Users { get; set; }
        public DbSet<Children> Childrens { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GrowthIndex> GrowthIndices { get; set; }
        public DbSet<HealthMetric> HealthMetrics { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> Methods { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> Plans { get; set; }
        public DbSet<TransactionHistory> Transactions { get; set; }
        public DbSet<WHOStandard> Standards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }   
}
