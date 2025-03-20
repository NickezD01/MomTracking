using Domain.Entity;
namespace Application.Response.SubscriptionPlan
{
    public class SubscriptionPlanResponse
    {
        public int Id { get; set; }
        public SubscriptionPlanName Name { get; set; }
        public decimal? Price { get; set; }
        public int DurationInMonths { get; set; }
        public string Description { get; set; }
        public string Feature { get; set; }
        public bool IsActive { get; set; }
        public int ActiveSubscribersCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}