using Application.Interface;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ExpiredSubscriptionService : IExpiredSubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExpiredSubscriptionService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CheckExpiredSub()
        {
            var today = DateTime.Now;
            var subs = await _unitOfWork.Subscriptions.GetAllAsync(s => s.Status == SubscriptionStatus.Active);
            foreach( var sub in subs)
            {              
                if(today.Date == sub.EndDate.Date)
                {
                    sub.Status = SubscriptionStatus.Expired;
                }
            }
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
