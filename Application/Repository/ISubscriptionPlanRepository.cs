using Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repository
{
    public interface ISubscriptionPlanRepository : IGenericRepository<SubscriptionPlan>
    {
        Task<List<SubscriptionPlan>> GetActivePlans();
        Task<SubscriptionPlan> GetPlanWithSubscribers(int planId);
        Task<bool> IsPlanActive(int planId);
        Task<List<SubscriptionPlan>> GetPlansByPriceRange(decimal minPrice, decimal maxPrice);
        Task<bool> IsPlanNameExists(string planName);
        Task<SubscriptionPlan> GetPlanByName(SubscriptionPlanName name);
        Task<List<SubscriptionPlan>> GetPlansByFeature(string feature);
        Task<int> GetTotalSubscribersCount(int planId);
    }
}