using Application.Request.Subscription;
using Application.Response;
using Application.Response.Subscription;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Application.Request.Subscription;
using Application.Response;

namespace Application.Interface
{
    public interface ISubscriptionService
    {
        // Subscription CRUD operations
        Task<ApiResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request);
        Task<ApiResponse> UpdateSubscriptionAsync(int Id,UpdateSubscriptionRequest request);
        Task<ApiResponse> CancelSubscriptionAsync(int subscriptionId);
        Task<ApiResponse> GetSubscriptionByIdAsync(int subscriptionId);
        Task<ApiResponse> DeleteSubPlanData(int Id);
        // User-related subscription operations
        Task<ApiResponse> GetUserSubscriptionsAsync(int accountId);
        Task<ApiResponse> GetActiveUserSubscriptionAsync(int accountId);
        
        // Subscription management operations
        Task<ApiResponse> ProcessSubscriptionPaymentAsync(int subscriptionId);
        
        // Subscription status operations
        Task<ApiResponse> CheckSubscriptionStatusAsync(int accountId);
        Task<ApiResponse> HandleExpiredSubscriptionsAsync();
        
    }
}