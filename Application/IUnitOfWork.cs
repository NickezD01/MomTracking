using Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IUnitOfWork
    {
        public IUserAccountRepository UserAccounts { get; }
        public IEmailVerificationRepository EmailVerifications { get; }
        public IChildrenRepository Childrens { get; }
        public ICommentRepository Comments { get; }
        public IGrowthIndexRepository GrowthIndex { get; }
        public IHeathMetricRepository HeathMetric { get; }
        public INotificationRepository Notification { get; }
        public IOrderRepository Order { get; }
        public IPostRepository Post { get; }
        public IScheduleRepository Schedule { get; }
        public ISubscriptionPlanRepository SubscriptionPlan { get; }
        public ISubscriptionRepository Subscription { get; }
        public ITransactionHistoryRepository TransactionHistory { get; }
        public IWHOStandardRepository WHOStandard { get; }
        public Task SaveChangeAsync();
    }
}
