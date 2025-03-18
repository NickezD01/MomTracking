using Application.Interface;
using Application.Request.SubscriptionPlan;
using Application.Response;
using Application.Response.SubscriptionPlan;
using AutoMapper;
using Domain.Entity;
using FluentValidation;
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
        private readonly IValidator<CreateSubscriptionPlanRequest> _subplanValidator;

        public SubscriptionPlanService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IClaimService claimService,
            IValidator<CreateSubscriptionPlanRequest> subplanValidator
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
            _claimService = claimService;
            _subplanValidator = subplanValidator;
        }

        public async Task<ApiResponse> CreatePlanAsync(CreateSubscriptionPlanRequest request)
        {
            ApiResponse apiResponse = new ApiResponse();
            var validationResult = _subplanValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return apiResponse.SetBadRequest(string.Join(", ", errors));
            }
            try
            {
                var userClaim = _claimService.GetUserClaim();
                if (userClaim.Role != Role.Manager)
                {
                    return apiResponse.SetBadRequest("Only managers can create subscription plans");
                }

                if (await _unitOfWork.SubscriptionPlans.IsPlanNameExists(request.Name))
                {
                    return apiResponse.SetBadRequest("A plan with this name already exists");
                }

                var plan = _mapper.Map<SubscriptionPlan>(request);

                // Đảm bảo isActive luôn là false khi tạo mới - ghi đè sau khi mapping
                plan.IsActive = true;

                await _unitOfWork.SubscriptionPlans.AddAsync(plan);
                await _unitOfWork.SaveChangeAsync();

                // Đảm bảo response cũng có isActive = false
                var response = _mapper.Map<SubscriptionPlanResponse>(plan);
                response.IsActive = true; // Đảm bảo response cũng có giá trị đúng

                return apiResponse.SetOk(response);
            }
            catch (Exception ex)
            {
                return apiResponse.SetBadRequest($"Error creating subscription plan: {ex.Message}");
            }
        }


        public async Task<ApiResponse> UpdatePlanAsync(int Id, UpdateSubscriptionPlanRequest request)
        {
            try
            {
                var userClaim = _claimService.GetUserClaim();
                if (userClaim.Role != Role.Manager)
                {
                    return new ApiResponse().SetBadRequest("Only managers can update subscription plans");
                }

                var plan = await _unitOfWork.SubscriptionPlans.GetAsync(p => p.Id == Id);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                _mapper.Map(request, plan);
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
                var plan = await _unitOfWork.SubscriptionPlans.GetAsync(p => p.Id == planId);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                // Check if plan has active subscribers
                var activeSubscribers = await _unitOfWork.SubscriptionPlans.GetTotalSubscribersCount(planId);
                if (activeSubscribers > 0)
                {
                    return new ApiResponse().SetBadRequest("Cannot delete plan with active subscribers");
                }

                plan.IsDeleted = true;
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
                var plan = await _unitOfWork.SubscriptionPlans.GetAsync(p => p.Id == planId && !p.IsDeleted);
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
                var plans = await _unitOfWork.SubscriptionPlans.GetAllAsync(p => !p.IsDeleted);
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
                var plans = await _unitOfWork.SubscriptionPlans.GetActivePlans();
                var response = _mapper.Map<List<SubscriptionPlanResponse>>(plans);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving active plans: {ex.Message}");
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

                var plan = await _unitOfWork.SubscriptionPlans.GetPlanWithSubscribers(planId);
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
                var plan = await _unitOfWork.SubscriptionPlans.GetAsync(p => p.Id == planId && !p.IsDeleted);
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

                var plan = await _unitOfWork.SubscriptionPlans.GetAsync(p => p.Id == planId);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                plan.IsActive = true;

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

                var plan = await _unitOfWork.SubscriptionPlans.GetAsync(p => p.Id == planId);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                plan.IsActive = false;
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

                var plan = await _unitOfWork.SubscriptionPlans.GetAsync(p => p.Id == planId);
                if (plan == null)
                {
                    return new ApiResponse().SetNotFound("Subscription plan not found");
                }

                plan.Price = newPrice;

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

                var planExists = await _unitOfWork.SubscriptionPlans.IsPlanNameExists(request.Name);
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

        //admindashboard
        public async Task<ApiResponse> CountPlan()
        {
            var plans = await _unitOfWork.SubscriptionPlans.GetAllAsync(null) ;
            var subs = await _unitOfWork.Subscriptions.GetAllAsync(s => s.PaymentStatus == PaymentStatus.Paid);
            var counts = new Dictionary<SubscriptionPlanName, int>();
            foreach(var plan in plans)
            {
                counts[plan.Name] = subs.Count(sub => sub.PlanId == plan.Id);
            }
            return new ApiResponse().SetOk(counts);
        }
        public async Task<ApiResponse> CalculateTotalRevenue()
        {
            var plans = await _unitOfWork.SubscriptionPlans.GetAllAsync(null);
            var subs = await _unitOfWork.Subscriptions.GetAllAsync(s => s.PaymentStatus == PaymentStatus.Paid);
            var userCounts = new Dictionary<SubscriptionPlanName, int>();
            foreach (var plan in plans)
            {
                userCounts[plan.Name] = subs.Count(sub => sub.PlanId == plan.Id);
            }
            var totalRevenue = new Dictionary<SubscriptionPlanName, decimal>();

            foreach (var plan in plans)
            {
                
                totalRevenue[plan.Name] = userCounts[plan.Name] * plan.Price;
            }


            return new ApiResponse().SetOk(totalRevenue);
        }
        public async Task<ApiResponse> TotalPrice()
        {
            var planBronze = await _unitOfWork.SubscriptionPlans.GetAsync(b => b.Name == SubscriptionPlanName.Bronze);
            var subs = await _unitOfWork.Subscriptions.GetAllAsync(s => s.PaymentStatus == PaymentStatus.Paid);
            int bronzeUser = 0;
            if (subs != null)
            {
                foreach (var sub in subs)
                {
                    if (sub.PlanId == planBronze.Id)
                    {
                        bronzeUser++;
                    }
                }
            }
            else
            {
                bronzeUser = 0;
            }


                var planSilver = await _unitOfWork.SubscriptionPlans.GetAsync(b => b.Name == SubscriptionPlanName.Silver);
            int silverUser = 0;
            if(subs != null)
            {
                foreach (var sub in subs)
                {
                    if (sub.PlanId == planSilver.Id)
                    {
                        silverUser++;
                    }
                }
            }
            else
            {
                silverUser = 0;
            }


                var planGold = await _unitOfWork.SubscriptionPlans.GetAsync(b => b.Name == SubscriptionPlanName.Gold);
            int goldUser = 0;
            if(subs != null)
            {
                foreach (var sub in subs)
                {
                    if (sub.PlanId == planGold.Id)
                    {
                        goldUser++;
                    }
                }
            }
            else
            {
                goldUser = 0;
            }

                var totalprice = (bronzeUser * planBronze.Price) + (silverUser * planSilver.Price) + (goldUser * planGold.Price);
            return new ApiResponse().SetOk(totalprice);
        }
    }
}
