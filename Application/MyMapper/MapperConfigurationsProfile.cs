
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
