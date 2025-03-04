namespace Application.Request.SubscriptionPlan
{
    public class UpdateSubscriptionPlanRequest
    {
        public int PlanId { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public string Feature { get; set; }
        public bool? IsActive { get; set; }
    }
}