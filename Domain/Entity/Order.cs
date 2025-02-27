using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid PlanId { get; set; }
        public double TotalPrice { get; set; }
        public string Note { get; set; }
        public bool IsDelete { get; set; }
        public UserAccount? Account { get; set; }
        public SubscriptionPlan? SubscriptionPlans { get; set; }
        public List<Payment>? Payments { get; set; }
    }
}
