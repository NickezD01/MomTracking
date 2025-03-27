using Domain.Entity;

namespace Application.Request.SubscriptionPlan
{
    public class UpdateSubscriptionPlanRequest
    {
        //public int PlanId { get; set; }
        public SubscriptionPlanName Name { get; set; }
        public decimal? Price { get; set; }
        public int DurationMonth { get; set; }
        public string Description { get; set; }
        public string Feature { get; set; }
        public bool? IsActive { get; set; }
    }
}