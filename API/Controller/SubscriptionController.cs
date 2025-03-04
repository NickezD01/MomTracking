using Application.Interface;
using Application.Request.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IClaimService _claimService;

        public SubscriptionController(
            ISubscriptionService subscriptionService,
            IClaimService claimService)
        {
            _subscriptionService = subscriptionService;
            _claimService = claimService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequest request)
        {
        var userClaim = _claimService.GetUserClaim();
            request.AccountId = userClaim.Id; // Set the authenticated user's ID
            
            var response = await _subscriptionService.CreateSubscriptionAsync(request);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

        [HttpGet("my-subscriptions")]
        public async Task<IActionResult> GetMySubscriptions()
    {
        var userClaim = _claimService.GetUserClaim();
            var response = await _subscriptionService.GetUserSubscriptionsAsync(userClaim.Id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

        [HttpGet("active")]
        public async Task<IActionResult> GetMyActiveSubscription()
    {
        var userClaim = _claimService.GetUserClaim();
            var response = await _subscriptionService.GetActiveUserSubscriptionAsync(userClaim.Id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

        [HttpGet("status")]
        public async Task<IActionResult> CheckSubscriptionStatus()
    {
            var userClaim = _claimService.GetUserClaim();
            var response = await _subscriptionService.CheckSubscriptionStatusAsync(userClaim.Id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

        [HttpGet("{subscriptionId}")]
        public async Task<IActionResult> GetSubscriptionById(int subscriptionId)
    {
            var response = await _subscriptionService.GetSubscriptionByIdAsync(subscriptionId);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

        [HttpPut("{subscriptionId}")]
        public async Task<IActionResult> UpdateSubscription(int subscriptionId, [FromBody] UpdateSubscriptionRequest request)
    {
            if (subscriptionId != request.SubscriptionId)
                return BadRequest("Subscription ID mismatch");

            var response = await _subscriptionService.UpdateSubscriptionAsync(request);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

        [HttpPost("{subscriptionId}/cancel")]
        public async Task<IActionResult> CancelSubscription(int subscriptionId)
    {
            var response = await _subscriptionService.CancelSubscriptionAsync(subscriptionId);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

        [HttpPost("{subscriptionId}/renew")]
        public async Task<IActionResult> RenewSubscription(int subscriptionId)
    {
            var response = await _subscriptionService.RenewSubscriptionAsync(subscriptionId);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

        [HttpPost("{subscriptionId}/upgrade")]
        public async Task<IActionResult> UpgradeSubscription(int subscriptionId, [FromBody] int newPlanId)
        {
            var response = await _subscriptionService.UpgradeSubscriptionPlanAsync(subscriptionId, newPlanId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
}
    [Authorize(Roles = "Manager")]
        [HttpGet("metrics")]
        public async Task<IActionResult> GetSubscriptionMetrics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
            var response = await _subscriptionService.GetSubscriptionMetricsAsync(startDate, endDate);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

        [Authorize(Roles = "Manager")]
        [HttpPost("process-expired")]
        public async Task<IActionResult> ProcessExpiredSubscriptions()
        {
            var response = await _subscriptionService.HandleExpiredSubscriptionsAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
}

        [Authorize(Roles = "Manager")]
        [HttpPost("process-renewals")]
        public async Task<IActionResult> ProcessSubscriptionRenewals()
        {
            var response = await _subscriptionService.HandleSubscriptionRenewalsAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
}
    }
}
