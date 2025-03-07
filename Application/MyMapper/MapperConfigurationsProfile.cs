
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



namespace Application.MyMapper
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            //UserAccount
            //CreateMap<UserProfileResponse, UserAccount>();
            CreateMap<UpdateUserRequest, UserAccount>();
            CreateMap<UserAccount, UserProfileResponse>();
            CreateMap<UserAccount, AccountResponse>();

            //Children
            CreateMap<ChildrenRequest, Children>();
            CreateMap<Children, ChildrenResponse>();
            CreateMap<ChildrenUpdateRequest, Children>();
            //Comment


            //GrowthIndex


            //HealthMetric
            CreateMap<HealthMetricRequest, HealthMetric>();
            CreateMap<HealthMetric, HealthMetricResponse>();
            CreateMap<HealthMetricUpdateRequest, HealthMetric>();
            //Notification


            //Order


            //Post


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
