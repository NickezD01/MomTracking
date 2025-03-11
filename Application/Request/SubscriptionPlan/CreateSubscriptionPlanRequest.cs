using Domain.Entity;

namespace Application.Request.SubscriptionPlan;

public class CreateSubscriptionPlanRequest
{
    public SubscriptionPlanName Name { get; set; }
    public decimal Price { get; set; }
    public int DurationInMonths { get; set; }
    public string Description { get; set; }
    public string Feature { get; set; }
    public string? IsActive { get; set; }
}