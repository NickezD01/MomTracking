using Application.Request.SubscriptionPlan;
using Application.Response;
using Application.Response.SubscriptionPlan;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ISubscriptionPlanService
    {
        // Plan CRUD operations
        Task<ApiResponse> CreatePlanAsync(CreateSubscriptionPlanRequest request);
        Task<ApiResponse> UpdatePlanAsync(int Id, UpdateSubscriptionPlanRequest request);
        Task<ApiResponse> DeletePlanAsync(int planId);
        Task<ApiResponse> GetPlanByIdAsync(int planId);
        
        // Plan listing operations
        Task<ApiResponse> GetAllPlansAsync();
        // Đã xóa: Task<ApiResponse> GetActivePlansAsync();
        
        // Plan detail operations


        
        // Plan management operations

    

        //Admin Dashboard

    }
}