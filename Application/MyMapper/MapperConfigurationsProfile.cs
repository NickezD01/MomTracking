
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
using Application.Request.Subscription;
using Application.Request.SubscriptionPlan;
using Application.Response.Subscription;
using Application.Response.SubscriptionPlan;


namespace Application.MyMapper
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            //UserAccount
            //CreateMap<UserProfileResponse, UserAccount>();
            //CreateMap<UpdateUserRequest, UserAccount>();
            CreateMap<UserAccount, UserProfileResponse>();
            CreateMap<UserAccount, AccountResponse>();

            //Children
            CreateMap<ChildrenRequest, Children>();
            CreateMap<Children, ChildrenResponse>();

            // SubscriptionPlan mappings
            CreateMap<CreateSubscriptionPlanRequest, SubscriptionPlan>()
                .ForMember(dest => dest.DurationMonth, opt => opt.MapFrom(src => src.DurationInMonths))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<SubscriptionPlan, SubscriptionPlanResponse>()
                .ForMember(dest => dest.DurationInMonths, opt => opt.MapFrom(src => src.DurationMonth))
                .ForMember(dest => dest.ActiveSubscribersCount, opt => 
                    opt.MapFrom(src => src.Subscriptions != null ? 
                        src.Subscriptions.Count(s => s.Status == "Active") : 0));

            CreateMap<SubscriptionPlan, SubscriptionPlanDetailResponse>()
                .ForMember(dest => dest.DurationInMonths, opt => opt.MapFrom(src => src.DurationMonth))
                .ForMember(dest => dest.ActiveSubscribers, opt => opt.MapFrom(src => 
                    src.Subscriptions.Where(s => s.Status == "Active")
                    .Select(s => new SubscriptionPlanDetailResponse.SubscriberInfo
                    {
                        AccountId = s.AccountId,
                        AccountName = s.Account.FirstName + " " + s.Account.LastName,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        Status = s.Status
                    })));
            
            // Subscription mappings
            CreateMap<CreateSubscriptionRequest, Subscription>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => "Pending"));

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

            CreateMap<UpdateSubscriptionRequest, Subscription>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            //Comment


            //GrowthIndex


            //HealthMetric


            //Notification


            //Order


            //Post


            //Schedule


            //SubcriptionPlan


            //Subscription


            //TransactionHistory


            //WHOStandaed

        }
    }
}
