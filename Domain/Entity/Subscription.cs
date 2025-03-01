using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Subscription
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool AutoRenewal { get; set; }
        public string Status { get; set; } // Active, Expired, Cancelled, PendingPayment
        public string PaymentStatus { get; set; } // Paid, Pending, Failed
        public DateTime? LastPaymentDate { get; set; }
        public DateTime? NextBillingDate { get; set; }
        public UserAccount? Account { get; set; }
        public SubscriptionPlan? SubscriptionPlans { get; set; }
    }
}
