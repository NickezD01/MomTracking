using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class SubscriptionPlan : Base
    {
        public Guid Id { get; set; }
        public SubscriptionPlanName Name { get; set; }

        public decimal Price { get; set; }
        public DateTime DurationMonth { get; set; }
        public string? Description { get; set; }
        public List<Subscription>? Subscriptions { get; set; }
        public List<Order>? Orders { get; set; }
    }
    public enum SubscriptionPlanName
    {
        Bronze,
        Silver,
        Gold
    }
}
