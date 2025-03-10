﻿using Application.Interface;
using Application.Request.Schedule;
using Application.Response;
using Application.Response.Schedule;
using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IClaimService _claim;
        private IEmailService _emailService;
        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IClaimService claim, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claim = claim;
            _emailService = emailService;
        }
        public async Task<ApiResponse> CreateSchedule(ScheduleRequest scheduleRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var claim = _claim.GetUserClaim();
                var user = await _unitOfWork.UserAccounts.GetAsync(a => a.Id == claim.Id); 
                var schedule = _mapper.Map<Schedule>(scheduleRequest);
                var currentDate = DateTime.Now.Date;
                schedule.AccountId = claim.Id;
                var scheduleExist = await _unitOfWork.Schedule.GetAsync(s => s.AppointmentDate == schedule.AppointmentDate);
                if (scheduleExist == null)
                {
                    await _unitOfWork.Schedule.AddAsync(schedule);
                    await _unitOfWork.SaveChangeAsync();
                    if (currentDate == schedule.AppointmentDate.Date.AddDays(-2))
                    {
                        var emailContent = EmailContentBuilder.BuildNotiMail(user.FirstName, schedule.AppointmentDate);
                        var emailResponse = await _emailService.SendNotiMail(user.Email, emailContent);
                    }
                    return apiResponse.SetOk("Schedule created successfully!!!");
                    

                }
                return apiResponse.SetBadRequest("Scheldule already exist!!!");
            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }

        public async Task<ApiResponse> DeleteSchedule(int Id)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var schedule = await _unitOfWork.Schedule.GetAsync(s => s.Id == Id);
                if (schedule == null)
                {
                    return apiResponse.SetNotFound("Schedule not created yet!!!");
                }
                await _unitOfWork.Schedule.RemoveByIdAsync(Id);
                await _unitOfWork.SaveChangeAsync();
                return apiResponse.SetOk("Deleted successfully!");
            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }

        public async Task<ApiResponse> GetAllSchedule()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var schedule = await _unitOfWork.Schedule.GetAllAsync(null);
                var resSchedule = _mapper.Map<List<ScheduleResponse>>(schedule);
                return new ApiResponse().SetOk(resSchedule);
            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }

        public async Task<ApiResponse> UpdateSchedule(int Id, ScheduleRequest scheduleRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var schedule = await _unitOfWork.Schedule.GetAsync(s => s.Id == Id);
                if(schedule == null)
                {
                    return apiResponse.SetNotFound("Schedule not created yet!!!");
                }
                _mapper.Map(scheduleRequest, schedule);
                await _unitOfWork.SaveChangeAsync();
                return apiResponse.SetOk("Schedule updated successfully!");
            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }
    }
}
