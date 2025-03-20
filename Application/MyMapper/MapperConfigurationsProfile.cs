using Application.Services;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Response.UserAccount;
using Application.Request.Children;
using Application.Response.Children;
using Application.Request.HealthMetric;
using Application.Response;
using Application.Request.UserAccount;
using Application.Request.WHO;
using Application.Response.WHO;
using Application.Request.Schedule;
using Application.Response.Schedule;
using Application.Request.Subscription;
using Application.Request.SubscriptionPlan;
using Application.Response.Subscription;
using Application.Response.SubscriptionPlan;
using Application.Request.Orders;
using Application.Response.Comment;
using Application.Response.Orders;
using Application.Response.Post;


namespace Application.MyMapper
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            //UserAccount
            //CreateMap<UserProfileResponse, UserAccount>();

            CreateMap<UpdateUserRoleRequest, UserAccount>();
            CreateMap<UpdateUserRequest, UserAccount>();
            CreateMap<UserAccount, UserProfileResponse>();
            CreateMap<UserAccount, AccountResponse>();

            //Children
            CreateMap<ChildrenRequest, Children>();
            CreateMap<Children, ChildrenResponse>();

            CreateMap<ChildrenUpdateRequest, Children>();


            // SubscriptionPlan mappings
            CreateMap<CreateSubscriptionPlanRequest, SubscriptionPlan>()
                .ForMember(dest => dest.DurationMonth, opt => opt.MapFrom(src => src.DurationInMonths))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateSubscriptionPlanRequest, SubscriptionPlan>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<SubscriptionPlan, SubscriptionPlanResponse>()
                .ForMember(dest => dest.DurationInMonths, opt => opt.MapFrom(src => src.DurationMonth))
                .ForMember(dest => dest.ActiveSubscribersCount, opt =>
                    opt.MapFrom(src => src.Subscriptions != null ?
                        src.Subscriptions.Count(s => s.Status == SubscriptionStatus.Active &&
                                     s.PaymentStatus == PaymentStatus.Paid &&
                                     s.EndDate > DateTime.Now) : 0));

            // Subscription mappings
            CreateMap<CreateSubscriptionRequest, Subscription>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => "Pending"))
                .ForMember(dest => dest.EndDate, opt => opt.Ignore()); // Will be calculated in service

            CreateMap<UpdateSubscriptionRequest, Subscription>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Subscription, SubscriptionResponse>()
                .ForMember(dest => dest.AccountName, opt => 
                    opt.MapFrom(src => src.Account != null ? 
                        $"{src.Account.FirstName} {src.Account.LastName}" : ""))
                .ForMember(dest => dest.PlanName, opt => 
                    opt.MapFrom(src => src.SubscriptionPlans != null ? 
                        src.SubscriptionPlans.Name.ToString() : ""))
                .ForMember(dest => dest.Price, opt => 
                    opt.MapFrom(src => src.SubscriptionPlans != null ? 
                        src.SubscriptionPlans.Price : 0))
                .ForMember(dest => dest.Features, opt => 
                    opt.MapFrom(src => src.SubscriptionPlans != null ? 
                        src.SubscriptionPlans.Feature : ""));

            //Comment
            CreateMap<Comment, CommentResponse>()
                .ForMember(dest => dest.AuthorName, opt => 
                    opt.MapFrom(src => src.Account != null ? 
                        $"{src.Account.FirstName} {src.Account.LastName}" : ""))
                .ForMember(dest => dest.AuthorImageUrl, opt => 
                    opt.MapFrom(src => src.Account != null ? src.Account.ImgUrl : null));

            //GrowthIndex


            //HealthMetric
            CreateMap<HealthMetricRequest, HealthMetric>();
            CreateMap<HealthMetric, HealthMetricResponse>();
            CreateMap<HealthMetricUpdateRequest, HealthMetric>();
            //Notification


            //Order
            CreateMap<OrderRequest, Order>();
            CreateMap<Order, OrderResponse>();
            //Post
            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.AuthorName, opt => 
                    opt.MapFrom(src => src.Account != null ? 
                        $"{src.Account.FirstName} {src.Account.LastName}" : ""))
                .ForMember(dest => dest.AuthorImageUrl, opt => 
                    opt.MapFrom(src => src.Account != null ? src.Account.ImgUrl : null))
                .ForMember(dest => dest.CommentCount, opt => 
                    opt.MapFrom(src => src.Comments != null ? 
                        src.Comments.Count(c => !c.IsDeleted) : 0))
                .ForMember(dest => dest.Comments, opt => 
                    opt.MapFrom(src => src.Comments != null ? 
                        src.Comments.Where(c => !c.IsDeleted).OrderByDescending(c => c.CreatedDate).Take(5) : null))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            //Schedule
            CreateMap<ScheduleRequest, Schedule>();
            CreateMap<Schedule, ScheduleResponse>();
            //SubcriptionPlan


            //Subscription


            //TransactionHistory


            //WHOStandaed
            CreateMap<StandardRequest, WHOStandard>();
            CreateMap<WHOStandard, StandardResponse>();
        }
    }
}
