namespace Application.Request.Subscription
{
    public class CreateSubscriptionRequest
    {
        //public int AccountId { get; set; }
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
    }
}