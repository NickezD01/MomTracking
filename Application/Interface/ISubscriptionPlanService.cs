using Application.Request.SubscriptionPlan;
using Application.Response;
using Application.Response.SubscriptionPlan;
using System.Threading.Tasks;
using Application.Request.SubscriptionPlan;
using Application.Response;

namespace Application.Interface
{
    public interface ISubscriptionPlanService
    {
        // Plan CRUD operations
        Task<ApiResponse> CreatePlanAsync(CreateSubscriptionPlanRequest request);
        Task<ApiResponse> UpdatePlanAsync(UpdateSubscriptionPlanRequest request);
        Task<ApiResponse> DeletePlanAsync(int planId);
        Task<ApiResponse> GetPlanByIdAsync(int planId);
        
        // Plan listing operations
        Task<ApiResponse> GetAllPlansAsync();
        Task<ApiResponse> GetActivePlansAsync();
        
        // Plan detail operations
        Task<ApiResponse> GetPlanDetailsWithSubscribersAsync(int planId);
        Task<ApiResponse> GetPlanFeaturesAsync(int planId);
        
        // Plan management operations
        Task<ApiResponse> ActivatePlanAsync(int planId);
        Task<ApiResponse> DeactivatePlanAsync(int planId);
        Task<ApiResponse> UpdatePlanPricingAsync(int planId, decimal newPrice);
        
        // Plan validation
        Task<ApiResponse> ValidatePlanAsync(CreateSubscriptionPlanRequest request);
    }
}