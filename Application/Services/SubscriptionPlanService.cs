using Application.Interface;
using Application.Request.SubscriptionPlan;
using Application.Response;
using Application.Response.SubscriptionPlan;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;

        public SubscriptionPlanService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IClaimService claimService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimService = claimService;
        }

        public async Task<ApiResponse> CreatePlanAsync(CreateSubscriptionPlanRequest request)
        {
            try
            {
                // Verify if user is Manager
                var userClaim = _claimService.GetUserClaim();
                if (userClaim.Role != Role.Manager)
                {
                    return new ApiResponse().SetBadRequest("Only managers can create subscription plans");
                }

                // Check if plan name already exists
                var planExists = await _unitOfWork.SubscriptionPlan.IsPlanNameExists(request.Name.ToString());
                if (planExists)
                {
                    return new ApiResponse().SetBadRequest("A plan with this name already exists");
                }

                // Create new plan
                var plan = new SubscriptionPlan
                {
                    Name = request.Name,
                    Price = request.Price,
                    DurationMonth = request.DurationInMonths,
                    Description = request.Description,
                    Feature = request.Feature,
                    IsActive = true,
                    CreatedBy = Guid.Parse(userClaim.Id.ToString()),
                    CreatedDate = DateTime.UtcNow
                };
                await _unitOfWork.SubscriptionPlan.AddAsync(plan);
                await _unitOfWork.SaveChangeAsync();

                var response = _mapper.Map<SubscriptionPlanResponse>(plan);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error creating subscription plan: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdatePlanAsync(UpdateSubscriptionPlanRequest request)
        {
            try
            {
                // Verify if user is Manager
                var userClaim = _claimService.GetUserClaim();
                if (userClaim.Role != Role.Manager)
                {
                    return new ApiResponse().SetBadRequest("Only managers can update subscription plans");
                }
                var plan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == request.PlanId);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                // Update plan details
                if (request.Price.HasValue)
                    plan.Price = request.Price.Value;
                
                if (!string.IsNullOrEmpty(request.Description))
                    plan.Description = request.Description;
                
                if (!string.IsNullOrEmpty(request.Feature))
                    plan.Feature = request.Feature;
                
                if (request.IsActive.HasValue)
                    plan.IsActive = request.IsActive.Value;

                plan.ModifiedBy = Guid.Parse(userClaim.Id.ToString());
                plan.ModifiedDate = DateTime.UtcNow;
                await _unitOfWork.SaveChangeAsync();

                var response = _mapper.Map<SubscriptionPlanResponse>(plan);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error updating subscription plan: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeletePlanAsync(int planId)
        {
            try
            {
                // Verify if user is Manager
                var userClaim = _claimService.GetUserClaim();
                if (userClaim.Role != Role.Manager)
                {
                    return new ApiResponse().SetBadRequest("Only managers can delete subscription plans");
                }
                var plan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == planId);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                // Check if plan has active subscribers
                var activeSubscribers = await _unitOfWork.SubscriptionPlan.GetTotalSubscribersCount(planId);
                if (activeSubscribers > 0)
                {
                    return new ApiResponse().SetBadRequest("Cannot delete plan with active subscribers");
                }

                plan.IsDeleted = true;
                plan.ModifiedBy = Guid.Parse(userClaim.Id.ToString());
                plan.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();
                return new ApiResponse().SetOk("Subscription plan deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error deleting subscription plan: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetPlanByIdAsync(int planId)
        {
            try
            {
                var plan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == planId && !p.IsDeleted);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                var response = _mapper.Map<SubscriptionPlanResponse>(plan);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving subscription plan: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetAllPlansAsync()
        {
            try
            {
                var plans = await _unitOfWork.SubscriptionPlan.GetAllAsync(p => !p.IsDeleted);
                var response = _mapper.Map<List<SubscriptionPlanResponse>>(plans);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving subscription plans: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetActivePlansAsync()
        {
            try
            {
                var plans = await _unitOfWork.SubscriptionPlan.GetActivePlans();
                var response = _mapper.Map<List<SubscriptionPlanResponse>>(plans);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving active plans: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetPlansByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            try
            {
                var plans = await _unitOfWork.SubscriptionPlan.GetPlansByPriceRange(minPrice, maxPrice);
                var response = _mapper.Map<List<SubscriptionPlanResponse>>(plans);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving plans by price range: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetPlanDetailsWithSubscribersAsync(int planId)
        {
            try
            {
                // Verify if user is Manager
                var userClaim = _claimService.GetUserClaim();
                if (userClaim.Role != Role.Manager)
                {
                    return new ApiResponse().SetBadRequest("Only managers can view subscriber details");
                }

                var plan = await _unitOfWork.SubscriptionPlan.GetPlanWithSubscribers(planId);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                var response = _mapper.Map<SubscriptionPlanDetailResponse>(plan);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving plan details: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetPlanFeaturesAsync(int planId)
        {
            try
            {
                var plan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == planId && !p.IsDeleted);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                return new ApiResponse().SetOk(new { Features = plan.Feature });
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving plan features: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ActivatePlanAsync(int planId)
        {
            try
            {
                // Verify if user is Manager
                var userClaim = _claimService.GetUserClaim();
                if (userClaim.Role != Role.Manager)
                {
                    return new ApiResponse().SetBadRequest("Only managers can activate subscription plans");
                }

                var plan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == planId);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                plan.IsActive = true;
                plan.ModifiedBy = Guid.Parse(userClaim.Id.ToString());
                plan.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();
                return new ApiResponse().SetOk("Plan activated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error activating plan: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeactivatePlanAsync(int planId)
        {
            try
            {
                // Verify if user is Manager
                var userClaim = _claimService.GetUserClaim();
                if (userClaim.Role != Role.Manager)
                {
                    return new ApiResponse().SetBadRequest("Only managers can deactivate subscription plans");
                }

                var plan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == planId);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                plan.IsActive = false;
                plan.ModifiedBy = Guid.Parse(userClaim.Id.ToString());
                plan.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();
                return new ApiResponse().SetOk("Plan deactivated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error deactivating plan: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdatePlanPricingAsync(int planId, decimal newPrice)
        {
            try
            {
                // Verify if user is Manager
                var userClaim = _claimService.GetUserClaim();
                if (userClaim.Role != Role.Manager)
                {
                    return new ApiResponse().SetBadRequest("Only managers can update plan pricing");
                }

                var plan = await _unitOfWork.SubscriptionPlan.GetAsync(p => p.Id == planId);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                plan.Price = newPrice;
                plan.ModifiedBy = Guid.Parse(userClaim.Id.ToString());
                plan.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();
                return new ApiResponse().SetOk("Plan price updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error updating plan price: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ValidatePlanAsync(CreateSubscriptionPlanRequest request)
        {
            try
            {
                if (request.Price <= 0)
                {
                    return new ApiResponse().SetBadRequest("Price must be greater than zero");
                }

                if (request.DurationInMonths <= 0)
                {
                    return new ApiResponse().SetBadRequest("Duration must be greater than zero");
                }

                var planExists = await _unitOfWork.SubscriptionPlan.IsPlanNameExists(request.Name.ToString());
                if (planExists)
                {
                    return new ApiResponse().SetBadRequest("A plan with this name already exists");
                }

                return new ApiResponse().SetOk("Plan validation successful");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error validating plan: {ex.Message}");
            }
        }
    }
}
