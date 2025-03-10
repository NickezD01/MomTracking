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
        public IGrowthIndexRepository GrowthIndexs { get; }
        public IHeathMetricRepository HeathMetrics { get; }
        public INotificationRepository Notifications { get; }
        public IOrderRepository Orders { get; }
        public IPostRepository Posts { get; }
        public IScheduleRepository Schedules { get; }
        public ISubscriptionPlanRepository SubscriptionPlans { get; }
        public ISubscriptionRepository Subscriptions { get; }
        public ITransactionHistoryRepository TransactionHistorys { get; }
        public IWHOStandardRepository WHOStandards { get; }
        public IPaymentRepository Payments { get; }
        public Task SaveChangeAsync();
    }
}
