using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int SubscriptionId { get; set; } // Thay đổi từ PlanId sang SubscriptionId
        public double TotalPrice { get; set; }
        public string Note { get; set; }
        public bool IsDelete { get; set; }
        public UserAccount? Account { get; set; }
        public Subscription? Subscription { get; set; } // Thay đổi từ SubscriptionPlans sang Subscription
        public List<Payment>? Payments { get; set; }
    }
}
