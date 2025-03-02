using System;
namespace Application.Request.Subscription
{
    public class UpdateSubscriptionRequest
    {
        public int SubscriptionId { get; set; }
        public int PlanId { get; set; }  // New plan if upgrading/downgrading
        public string Status { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
