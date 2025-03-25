using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository
{
    public interface IUserAccountRepository: IGenericRepository<UserAccount>
    {
        Task<List<UserAccount>> GetActiveUserAccountsAsync();
        Task<List<UserAccount>> GetUserAccountsByPlanNameAsync(SubscriptionPlanName name);
    }
}
