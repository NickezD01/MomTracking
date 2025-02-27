using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
        public UserAccount? Account { get; set; }
        public SubscriptionPlan? SubscriptionPlans { get; set; }
    }
}
