using Application.Interface;
using Application.Request.Subscription;
using Application.Response;
using Application.Response.Subscription;
using AutoMapper;
using Domain.Entity;
using MaxMind.GeoIP2.Responses;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var claim = _claimService.GetUserClaim();

                var user = await _unitOfWork.UserAccounts.GetAsync(u => u.Id == claim.Id);
                if (user == null)
                    return apiResponse.SetNotFound("User not found");

                var plan = await _unitOfWork.SubscriptionPlans.GetAsync(p => p.Id == request.PlanId);
                if (plan == null || !plan.IsActive)
                    return apiResponse.SetNotFound("Subscription plan not found or inactive");

                if (await _unitOfWork.Subscriptions.HasActiveSubscription(claim.Id))
                    return apiResponse.SetBadRequest("User already has an active subscription");

                var subscription = _mapper.Map<Subscription>(request);
                subscription.AccountId = claim.Id;
                subscription.Price = plan.Price;
                subscription.EndDate = request.StartDate.AddMonths(plan.DurationMonth);
                subscription.NextBillingDate = subscription.EndDate;

                await _unitOfWork.Subscriptions.AddAsync(subscription);
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
                var subscription = await _unitOfWork.Subscriptions.GetSubscriptionWithDetails(Id);
                if (subscription == null)
                    return new ApiResponse().SetNotFound("Subscription not found");

                if (request.PlanId != subscription.PlanId)
                {
                    var newPlan = await _unitOfWork.SubscriptionPlans.GetAsync(p => p.Id == Id);
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
                var subscription = await _unitOfWork.Subscriptions.GetSubscriptionWithDetails(subscriptionId);
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
                var subscription = await _unitOfWork.Subscriptions.GetSubscriptionWithDetails(subscriptionId);
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
                var subscriptions = await _unitOfWork.Subscriptions.GetSubscriptionHistory(accountId);
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
                var subscriptions = await _unitOfWork.Subscriptions.GetActiveSubscriptionsByAccountId(accountId);
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
                var expiringSubscriptionss = await _unitOfWork.Subscriptions
                    .GetExpiringSubscriptions(DateTime.UtcNow);

                foreach (var subscription in expiringSubscriptionss)
                {
                    subscription.Status = "Expired";
                }

                await _unitOfWork.SaveChangeAsync();
                return new ApiResponse().SetOk($"Processed {expiringSubscriptionss.Count} expired subscriptions");
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
                var hasActiveSubscription = await _unitOfWork.Subscriptions.HasActiveSubscription(accountId);
                return new ApiResponse().SetOk(new { HasActiveSubscription = hasActiveSubscription });
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error checking subscription status: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteSubPlanData(int Id)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var children = await _unitOfWork.SubscriptionPlans.GetAsync(c => c.Id == Id);
                if (children == null)
                {
                    return apiResponse.SetNotFound("Can not found the Children detail");
                }
                await _unitOfWork.SubscriptionPlans.RemoveByIdAsync(Id);
                await _unitOfWork.SaveChangeAsync();
                return apiResponse.SetOk("Deleled successfully!");


            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }
    }
}