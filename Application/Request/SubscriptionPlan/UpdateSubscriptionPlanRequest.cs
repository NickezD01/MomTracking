using Domain.Entity;

namespace Application.Request.SubscriptionPlan
{
    public class UpdateSubscriptionPlanRequest
    {
        //public int PlanId { get; set; }
<<<<<<< HEAD

        public decimal? Price { get; set; }
        //public int DurationInMonths { get; set; }
=======
        public SubscriptionPlanName Name { get; set; }
        public decimal? Price { get; set; }
        public int DurationMonth { get; set; }
>>>>>>> 81429b27f40eefc599f9788a8698f85a39d96d55
        public string Description { get; set; }
        public string Feature { get; set; }
        public bool? IsActive { get; set; }
    }
}