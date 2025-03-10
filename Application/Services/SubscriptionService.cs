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
        private readonly IClaimService _claimService;

        public SubscriptionService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IClaimService claimService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimService = claimService;
        }

        public async Task<ApiResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request)
        {
            try
            {
                var user = await _unitOfWork.UserAccounts.GetAsync(u => u.Id == request.AccountId);
                if (user == null)
                    return new ApiResponse().SetNotFound("User not found");

                var plan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == request.PlanId);
                if (plan == null || !plan.IsActive)
                    return new ApiResponse().SetNotFound("Subscription plan not found or inactive");

                if (await _unitOfWork.Subscription.HasActiveSubscription(request.AccountId))
                    return new ApiResponse().SetBadRequest("User already has an active subscription");

                var subscription = _mapper.Map<Subscription>(request);
                subscription.EndDate = request.StartDate.AddMonths(plan.DurationMonth);
                subscription.NextBillingDate = subscription.EndDate;

                await _unitOfWork.Subscription.AddAsync(subscription);
                await _unitOfWork.SaveChangeAsync();

                var response = _mapper.Map<SubscriptionResponse>(subscription);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error creating subscription: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateSubscriptionAsync(int Id, UpdateSubscriptionRequest request)
        {
            try
            {
                var subscription = await _unitOfWork.Subscription.GetSubscriptionWithDetails(Id);
                if (subscription == null)
                    return new ApiResponse().SetNotFound("Subscription not found");

                if (request.PlanId != subscription.PlanId)
                {
                    var newPlan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == Id);
                    if (newPlan == null || !newPlan.IsActive)
                        return new ApiResponse().SetBadRequest("Invalid subscription plan");
                }

                _mapper.Map(request, subscription);
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
                }

                await _unitOfWork.SaveChangeAsync();
                return new ApiResponse().SetOk($"Processed {expiringSubscriptions.Count} expired subscriptions");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error handling expired subscriptions: {ex.Message}");
            }
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

    }
}