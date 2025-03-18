using Domain.Entity;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace API.Middleware
{
    public class RequirePaidStatusAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int accountId))
            {
                context.Result = new UnauthorizedResult(); // Trả về 401 nếu không có UserId hợp lệ
                return;
            }

            // Lấy DbContext từ HttpContext.RequestServices
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            var subscription = await dbContext.Subscriptions
                .Where(s => s.AccountId == accountId && s.Status == SubscriptionStatus.Active)
                .OrderByDescending(s => s.StartDate) // Lấy gói đăng ký mới nhất
                .FirstOrDefaultAsync();

            if (subscription == null || subscription.PaymentStatus != PaymentStatus.Paid)
            {
                context.Result = new ForbidResult(); // Trả về 403 nếu chưa thanh toán
                return;
            }
        }
    }
}
