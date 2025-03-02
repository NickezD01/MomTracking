using Application.Response;
using System;
using System.Threading.Tasks;
namespace Application.Interface
{
    public interface ISubscriptionNotificationService
    {
        Task<ApiResponse> SendSubscriptionConfirmationAsync(int subscriptionId);
        Task<ApiResponse> SendRenewalReminderAsync(int subscriptionId);
        Task<ApiResponse> SendExpirationNoticeAsync(int subscriptionId);
        Task<ApiResponse> SendPaymentFailedNotificationAsync(int subscriptionId);
        Task<ApiResponse> SendUpgradeConfirmationAsync(int subscriptionId, int oldPlanId, int newPlanId);
        Task<ApiResponse> SendCancellationConfirmationAsync(int subscriptionId);
        Task<ApiResponse> ProcessPendingNotificationsAsync();
    }
}
