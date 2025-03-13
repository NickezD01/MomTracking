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
            var dbContext = context.HttpContext.RequestServices.GetService<AppDbContext>();
            var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int accountId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var payment = await dbContext.Payments
                .Where(p => p.AccountId == accountId)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            if (payment == null || payment.StatusPayment != StatusPayment.Paid)
            {
                context.Result = new ForbidResult(); // Trả về 403 Forbidden nếu chưa thanh toán
                return;
            }
        }
    }
}
