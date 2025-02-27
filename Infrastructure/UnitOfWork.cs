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

        public ICommentRepository Comments { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            UserAccounts = new UserAccountRepository(context);
            EmailVerifications = new EmailVerificationRepository(context);
            Childrens = new ChildrenRepository(context);
            Comments = new CommentRepository(context);
            GrowthIndex = new GrowthIndexRepository(context);
            HeathMetric = new HeathMetricRepository(context);
            Notification = new NotificationRepository(context);
            Order = new OrderRepository(context);
            Post = new PostRepository(context);
            Schedule = new ScheduleRepository(context);
            SubscriptionPlan = new SubscriptionPlanRepository(context);
            Subscription = new SubscriptionRepository(context);
            TransactionHistory = new TransactionHistoryRepository(context);
            WHOStandard = new WHOStandardRepository(context);
            


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
