using Domain.Entity;

namespace Application.Response.SubscriptionPlan;

public class SubscriptionPlanDetailResponse
{
    public int Id { get; set; }
    public SubscriptionPlanName Name { get; set; }
    public decimal? Price { get; set; }
    public int DurationInMonths { get; set; }
    public string Description { get; set; }
    public string Feature { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public List<SubscriberInfo> ActiveSubscribers { get; set; }
    
    public class SubscriberInfo
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}