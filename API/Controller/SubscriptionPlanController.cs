using Application.Interface;
using Application.Request.SubscriptionPlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlans()
        {
            var response = await _subscriptionPlanService.GetAllPlansAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActivePlans()
        {
            var response = await _subscriptionPlanService.GetActivePlansAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{planId}")]
        public async Task<IActionResult> GetPlanById(int planId)
        {
            var response = await _subscriptionPlanService.GetPlanByIdAsync(planId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("price-range")]
        public async Task<IActionResult> GetPlansByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var response = await _subscriptionPlanService.GetPlansByPriceRangeAsync(minPrice, maxPrice);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreatePlan([FromBody] CreateSubscriptionPlanRequest request)
        {
            var response = await _subscriptionPlanService.CreatePlanAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{planId}")]
        public async Task<IActionResult> UpdatePlan(int planId, [FromBody] UpdateSubscriptionPlanRequest request)
        {
            if (planId != request.PlanId)
                return BadRequest("Plan ID mismatch");

            var response = await _subscriptionPlanService.UpdatePlanAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{planId}")]
        public async Task<IActionResult> DeletePlan(int planId)
        {
            var response = await _subscriptionPlanService.DeletePlanAsync(planId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("{planId}/subscribers")]
        public async Task<IActionResult> GetPlanSubscribers(int planId)
        {
            var response = await _subscriptionPlanService.GetPlanDetailsWithSubscribersAsync(planId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{planId}/features")]
        public async Task<IActionResult> GetPlanFeatures(int planId)
        {
            var response = await _subscriptionPlanService.GetPlanFeaturesAsync(planId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPatch("{planId}/activate")]
        public async Task<IActionResult> ActivatePlan(int planId)
        {
            var response = await _subscriptionPlanService.ActivatePlanAsync(planId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPatch("{planId}/deactivate")]
        public async Task<IActionResult> DeactivatePlan(int planId)
        {
            var response = await _subscriptionPlanService.DeactivatePlanAsync(planId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPatch("{planId}/price")]
        public async Task<IActionResult> UpdatePlanPrice(int planId, [FromBody] decimal newPrice)
        {
            var response = await _subscriptionPlanService.UpdatePlanPricingAsync(planId, newPrice);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}