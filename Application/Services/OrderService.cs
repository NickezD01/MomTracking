using Application.Interface;
using Application.Response;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimService _claimService;

        public OrderService(IUnitOfWork unitOfWork, IClaimService claimService)
        {
            _unitOfWork = unitOfWork;
            _claimService = claimService;
        }

        public async Task<ApiResponse> CreateOrderFromSubscription(int subscriptionId)
        {
            ApiResponse response = new ApiResponse();
            var claim = _claimService.GetUserClaim();
            var userId = claim.Id;

            // Lấy thông tin subscription
            var subscription = await _unitOfWork.Subscriptions.GetSubscriptionWithDetails(subscriptionId);
            if (subscription == null)
            {
                return response.SetNotFound("Subscription not found");
            }

            // Kiểm tra subscription có hợp lệ không
            if (subscription.Status != "Active")
            {
                return response.SetBadRequest("Subscription is not active");
            }

            // Lấy thông tin gói subscription
            var plan = await _unitOfWork.SubscriptionPlans.GetAsync(x => x.Id == subscription.PlanId);
            if (plan == null)
            {
                return response.SetNotFound("Subscription plan not found");
            }

            // Tạo đơn hàng mới dựa trên subscription
            var order = new Order
            {
                AccountId = userId,
                SubscriptionId = subscriptionId,
                TotalPrice = plan.Price,
                Status = OrderStatus.Pending // Chờ thanh toán qua VNPay
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangeAsync();

            return response.SetOk(new { order.Id, order.TotalPrice });
        }

        public async Task<ApiResponse> GetOrderById(int orderId)
        {
            ApiResponse response = new ApiResponse();

            var order = await _unitOfWork.Orders.GetAsync(x => x.Id == orderId);
            if (order == null)
            {
                return response.SetNotFound("Order not found");
            }

            return response.SetOk(order);
        }

        public async Task<ApiResponse> GetUserOrders()
        {
            ApiResponse response = new ApiResponse();
            var claim = _claimService.GetUserClaim();
            var userId = claim.Id;

            var orders = await _unitOfWork.Orders.GetAllAsync(x => x.AccountId == userId);
            return response.SetOk(orders);
        }

        public async Task<ApiResponse> CancelOrder(int orderId)
        {
            ApiResponse response = new ApiResponse();
            var claim = _claimService.GetUserClaim();
            var userId = claim.Id;

            var order = await _unitOfWork.Orders.GetAsync(x => x.Id == orderId && x.AccountId == userId);
            if (order == null)
            {
                return response.SetNotFound("Order not found or does not belong to the user.");
            }

            if (order.Status == OrderStatus.Paid)
            {
                return response.SetBadRequest("Cannot cancel a paid order.");
            }

            order.Status = OrderStatus.Canceled;
            await _unitOfWork.SaveChangeAsync();

            return response.SetOk("Order canceled successfully.");
        }
    }
}
