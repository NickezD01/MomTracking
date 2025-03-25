using Application.Repository;
using DocumentFormat.OpenXml.VariantTypes;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserAccountRepository : GenericRepository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(AppDbContext context) : base(context)
        {
            
        }
        public async Task<List<UserAccount>> GetActiveUserAccountsAsync()
        {
            var activeSubscriptions = await _context.Subscriptions
                .Where(s => s.Status == SubscriptionStatus.Active)
                .Select(s => s.AccountId)
                .ToListAsync();

            var activeUserAccounts = await _context.Users
                .Where(u => activeSubscriptions.Contains(u.Id))
                .ToListAsync();

            return activeUserAccounts;
        }
        public async Task<List<UserAccount>> GetUserAccountsByPlanNameAsync(SubscriptionPlanName name)
        {
         var bronzePlanIds = await _context.Plans
            .Where(p => p.Name == name)
            .Select(p => p.Id)
            .ToListAsync();

        var activeSubscriptions = await _context.Subscriptions
            .Where(s => s.Status == SubscriptionStatus.Active && s.PaymentStatus == PaymentStatus.Paid && bronzePlanIds.Contains(s.PlanId))
            .Select(s => s.AccountId)
            .ToListAsync();
        var activeUserAccounts = await _context.Users
            .Where(u => activeSubscriptions.Contains(u.Id))
            .ToListAsync();
            return activeUserAccounts;
        }
    }
}
