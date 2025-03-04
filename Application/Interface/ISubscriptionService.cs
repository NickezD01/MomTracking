﻿using Application.Request.Subscription;
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
        Task<ApiResponse> UpdateSubscriptionAsync(UpdateSubscriptionRequest request);
        Task<ApiResponse> CancelSubscriptionAsync(int subscriptionId);
        Task<ApiResponse> GetSubscriptionByIdAsync(int subscriptionId);
        
        // User-related subscription operations
        Task<ApiResponse> GetUserSubscriptionsAsync(int accountId);
        Task<ApiResponse> GetActiveUserSubscriptionAsync(int accountId);
        
        // Subscription management operations
        Task<ApiResponse> RenewSubscriptionAsync(int subscriptionId);
        Task<ApiResponse> UpgradeSubscriptionPlanAsync(int subscriptionId, int newPlanId);
        Task<ApiResponse> ProcessSubscriptionPaymentAsync(int subscriptionId);
        
        // Subscription status operations
        Task<ApiResponse> CheckSubscriptionStatusAsync(int accountId);
        Task<ApiResponse> HandleExpiredSubscriptionsAsync();
        Task<ApiResponse> HandleSubscriptionRenewalsAsync();
        
        // Subscription analytics
        Task<ApiResponse> GetSubscriptionMetricsAsync(DateTime startDate, DateTime endDate);
    }
}