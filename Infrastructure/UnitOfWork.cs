using Application;
using Application.Repository;
using Domain.Entity;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork: IUnitOfWork
    {
        private AppDbContext _context;
        public IUserAccountRepository UserAccounts { get; }
        public IEmailVerificationRepository EmailVerifications { get; }

        public IChildrenRepository Childrens { get; }

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

        public ICommentRepository Comments { get; }
        public IPaymentRepository Payments { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            UserAccounts = new UserAccountRepository(context);
            EmailVerifications = new EmailVerificationRepository(context);
            Childrens = new ChildrenRepository(context);
            Comments = new CommentRepository(context);
            GrowthIndexs = new GrowthIndexRepository(context);
            HeathMetrics = new HeathMetricRepository(context);
            Notifications = new NotificationRepository(context);
            Orders = new OrderRepository(context);
            Posts = new PostRepository(context);
            Schedules = new ScheduleRepository(context);
            SubscriptionPlans = new SubscriptionPlanRepository(context);
            Subscriptions = new SubscriptionRepository(context);
            TransactionHistorys = new TransactionHistoryRepository(context);
            WHOStandards = new WHOStandardRepository(context);
            Payments = new PaymentRepository(context);



        }
        public async Task SaveChangeAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
    
}
