using Application.Interface;
using Application.Request.Subscription;
using Application.Response;
using Application.Response.Subscription;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISubscriptionNotificationService _notificationService;
        private readonly IClaimService _claimService;

        public SubscriptionService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ISubscriptionNotificationService notificationService,
            IClaimService claimService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _claimService = claimService;
        }

        public async Task<ApiResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request)
        {
            try
            {
                // Validate user and plan
                var user = await _unitOfWork.UserAccounts.GetAsync(u => u.Id == request.AccountId);
                if (user == null)
                    return new ApiResponse().SetNotFound("User not found");

                var plan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == request.PlanId);
                if (plan == null || !plan.IsActive)
                    return new ApiResponse().SetNotFound("Subscription plan not found or inactive");

                // Check if user has active subscription
                var hasActiveSubscription = await _unitOfWork.Subscription.HasActiveSubscription(request.AccountId);
                if (hasActiveSubscription)
                    return new ApiResponse().SetBadRequest("User already has an active subscription");

                // Calculate subscription period
                var startDate = request.StartDate;
                var endDate = startDate.AddMonths(plan.DurationMonth);

                // Create new subscription
                var subscription = new Subscription
                {
                    AccountId = request.AccountId,
                    PlanId = request.PlanId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Status = "Active",
                    PaymentStatus = "Pending",
                    NextBillingDate = endDate
                };

                await _unitOfWork.Subscription.AddAsync(subscription);
                await _unitOfWork.SaveChangeAsync();

                // Send confirmation notification
                await _notificationService.SendSubscriptionConfirmationAsync(subscription.Id);

                var response = _mapper.Map<SubscriptionResponse>(subscription);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error creating subscription: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateSubscriptionAsync(UpdateSubscriptionRequest request)
        {
            try
            {
                var subscription = await _unitOfWork.Subscription.GetSubscriptionWithDetails(request.SubscriptionId);
                if (subscription == null)
                    return new ApiResponse().SetNotFound("Subscription not found");

                // Update subscription details
                if (request.PlanId != subscription.PlanId)
                {
                    var newPlan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == request.PlanId);
                    if (newPlan == null || !newPlan.IsActive)
                        return new ApiResponse().SetBadRequest("Invalid subscription plan");

                    subscription.PlanId = request.PlanId;
                }

                subscription.Status = request.Status ?? subscription.Status;
                subscription.EndDate = request.EndDate ?? subscription.EndDate;
                subscription.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();

                var response = _mapper.Map<SubscriptionResponse>(subscription);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error updating subscription: {ex.Message}");
            }
        }

        public async Task<ApiResponse> CancelSubscriptionAsync(int subscriptionId)
        {
            try
            {
                var subscription = await _unitOfWork.Subscription.GetSubscriptionWithDetails(subscriptionId);
                if (subscription == null)
                    return new ApiResponse().SetNotFound("Subscription not found");

                subscription.Status = "Cancelled";
                subscription.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();
                await _notificationService.SendCancellationConfirmationAsync(subscriptionId);

                return new ApiResponse().SetOk("Subscription cancelled successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error cancelling subscription: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetSubscriptionByIdAsync(int subscriptionId)
        {
            try
            {
                var subscription = await _unitOfWork.Subscription.GetSubscriptionWithDetails(subscriptionId);
                if (subscription == null)
                    return new ApiResponse().SetNotFound("Subscription not found");

                var response = _mapper.Map<SubscriptionResponse>(subscription);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving subscription: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetUserSubscriptionsAsync(int accountId)
        {
            try
            {
                var subscriptions = await _unitOfWork.Subscription.GetSubscriptionHistory(accountId);
                var response = _mapper.Map<List<SubscriptionResponse>>(subscriptions);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving user subscriptions: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetActiveUserSubscriptionAsync(int accountId)
        {
            try
            {
                var subscriptions = await _unitOfWork.Subscription.GetActiveSubscriptionsByAccountId(accountId);
                if (!subscriptions.Any())
                    return new ApiResponse().SetNotFound("No active subscription found");

                var response = _mapper.Map<SubscriptionResponse>(subscriptions.First());
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving active subscription: {ex.Message}");
            }
        }

        public async Task<ApiResponse> RenewSubscriptionAsync(int subscriptionId)
        {
            try
            {
                var subscription = await _unitOfWork.Subscription.GetSubscriptionWithDetails(subscriptionId);
                if (subscription == null)
                    return new ApiResponse().SetNotFound("Subscription not found");

                var plan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == subscription.PlanId);
                if (plan == null || !plan.IsActive)
                    return new ApiResponse().SetBadRequest("Subscription plan is no longer active");

                // Create new subscription period
                var newStartDate = subscription.EndDate;
                var newEndDate = newStartDate.AddMonths(plan.DurationMonth);

                var newSubscription = new Subscription
                {
                    AccountId = subscription.AccountId,
                    PlanId = subscription.PlanId,
                    StartDate = newStartDate,
                    EndDate = newEndDate,
                    Status = "Active",
                    PaymentStatus = "Pending",
                    NextBillingDate = newEndDate
                };

                await _unitOfWork.Subscription.AddAsync(newSubscription);
                await _unitOfWork.SaveChangeAsync();

                await _notificationService.SendSubscriptionConfirmationAsync(newSubscription.Id);

                var response = _mapper.Map<SubscriptionResponse>(newSubscription);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error renewing subscription: {ex.Message}");
            }
        }

        public Task<ApiResponse> UpgradeSubscriptionPlanAsync(int subscriptionId, int newPlanId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> ProcessSubscriptionPaymentAsync(int subscriptionId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> HandleExpiredSubscriptionsAsync()
        {
            try
            {
                var expiringSubscriptions = await _unitOfWork.Subscription
                    .GetExpiringSubscriptions(DateTime.UtcNow);

                foreach (var subscription in expiringSubscriptions)
                {
                    subscription.Status = "Expired";
                    await _notificationService.SendExpirationNoticeAsync(subscription.Id);
                }

                await _unitOfWork.SaveChangeAsync();
                return new ApiResponse().SetOk($"Processed {expiringSubscriptions.Count} expired subscriptions");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error handling expired subscriptions: {ex.Message}");
            }
        }

        public Task<ApiResponse> HandleSubscriptionRenewalsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetSubscriptionMetricsAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> CheckSubscriptionStatusAsync(int accountId)
        {
            try
            {
                var hasActiveSubscription = await _unitOfWork.Subscription.HasActiveSubscription(accountId);
                return new ApiResponse().SetOk(new { HasActiveSubscription = hasActiveSubscription });
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error checking subscription status: {ex.Message}");
            }
        }

        // Additional methods implementation...
    }
}
