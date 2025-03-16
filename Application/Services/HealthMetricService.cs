﻿using Application.Interface;
using Application.Request.Children;
using Application.Request.HealthMetric;
using Application.Response;
using Application.Response.Children;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class HealthMetricService : IHeathMetricService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public HealthMetricService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddHealthMetric(HealthMetricRequest healthMetricRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var healthMetric = _mapper.Map<HealthMetric>(healthMetricRequest);
                healthMetric.ChildrentId = healthMetricRequest.ChildrentId;
                var DetailsExist = await _unitOfWork.HeathMetrics.GetAsync(x => x.PregnancyWeek == healthMetric.PregnancyWeek);
                if (DetailsExist == null || DetailsExist.ChildrentId != healthMetric.ChildrentId)
                {
                    if(healthMetric.PregnancyWeek > 7 && healthMetric.PregnancyWeek <= 40)
                    {
                        await _unitOfWork.HeathMetrics.AddAsync(healthMetric);
                        await _unitOfWork.SaveChangeAsync();
                        return apiResponse.SetOk("Children's health details added successfully!");
                    }
                    if(healthMetric.PregnancyWeek < 4)
                    {
                        return apiResponse.SetBadRequest("The fetus is in a stage of development without any specific signs!!!");
                    }
                    return apiResponse.SetBadRequest("Invalid PregnancyWeek!!!");
                }
                return apiResponse.SetBadRequest("You have entered this week's data");
            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }


        public async Task<ApiResponse> CompareData(int Id)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {

                var healthMetric = await _unitOfWork.HeathMetrics.GetAsync(c => c.Id == Id && c.Status == true);
                var standard = await _unitOfWork.WHOStandards.GetAsync(s => s.PregnancyWeek == healthMetric.PregnancyWeek);
                if (standard == null)
                {
                    return apiResponse.SetNotFound("The pregnancy is still in its development cycle and there is no specific data yet!");
                }
                if (healthMetric == null)
                {
                    return apiResponse.SetNotFound("Can not found the Children's health detail");
                }
                List<string> warnings = new List<string>();
                //tu tuan 8 toi tuan 14 cac sieu am se lay chi so chieu dai cua thai nhi
                if (healthMetric.PregnancyWeek >= 8 && healthMetric.PregnancyWeek <= 14)
                {
                    if(healthMetric.Lenght < standard.LenghtMin || healthMetric.Lenght > standard.LenghtMax)
                    {
                        warnings.Add("WARNING: Fetal lenght is different from the standard index!!!");
                    }
                }
                //tu tuan 14 tro di chieu dai thai nhi se ko dc khao sat moi lan sieu am
                if (healthMetric.PregnancyWeek > 14 && healthMetric.PregnancyWeek <= 15)
                {
                    if(healthMetric.Weight < standard.WeightMin || healthMetric.Weight > standard.WeightMax)
                    {
                        warnings.Add("WARNING: Fetal weight is different from the standard index!!!");
                    }
                   

                }
                //Tuan 16 tro di se co them nhung chi so can quan tam
                if (healthMetric.PregnancyWeek >= 16 && healthMetric.PregnancyWeek <= 40)
                {
                    if (healthMetric.Weight < standard.WeightMin || healthMetric.Weight > standard.WeightMax)
                    {
                        warnings.Add("WARNING: Fetal weight is different from the standard index!!!");
                    }
                    if (healthMetric.BPD < standard.BPDMin || healthMetric.BPD > standard.BPDMax)
                    {
                        warnings.Add("WARNING: Fetal biparietal diameter is different from the standard index!!!");
                    }
                    if (healthMetric.FL < standard.FLMin || healthMetric.FL > standard.FLMax)
                    {
                        warnings.Add("WARNING: Fetal femur length is different from the standard index!!!");
                    }
                    if (healthMetric.HearRate < standard.HearRateMin || healthMetric.HearRate > standard.HearRateMax)
                    {
                        warnings.Add("WARNING: The fetal heart rate is different from the standard index!!!");
                    }
                    if (healthMetric.HeadCircumference < standard.HeadCircumferenceMin || healthMetric.HeadCircumference > standard.HeadCircumferenceMax)
                    {
                        warnings.Add("WARNING: Fetal head circumference is different from the standard index!!!");
                    }
                    if (healthMetric.AC < standard.ACMin || healthMetric.AC > standard.ACMax)
                    {
                        warnings.Add("WARNING: The fetal heart rate is different from the standard index!!!");
                    }
                }
                if (warnings.Count > 0)
                {
                    healthMetric.IsAlert = true;
                    await _unitOfWork.SaveChangeAsync();
                    return apiResponse.SetOk(string.Join("\n",warnings));
                }
                else
                {
                    healthMetric.IsAlert = false;
                    await _unitOfWork.SaveChangeAsync();
                }

                return apiResponse.SetOk("The fetus's health is in the most stable condition");

            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }

        }

        public async Task<ApiResponse> DeleteHealthMetric(int Id)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var healthMetric = await _unitOfWork.HeathMetrics.GetAsync(c => c.Id == Id);
                if (healthMetric == null)
                {
                    return apiResponse.SetNotFound("Can not found the Children's Health detail");
                }
                await _unitOfWork.HeathMetrics.RemoveByIdAsync(Id);
                await _unitOfWork.SaveChangeAsync();
                return apiResponse.SetOk("Delele successfully!");


            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }

        public async Task<ApiResponse> GetAllHealthMetric()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var healthMetric = await _unitOfWork.HeathMetrics.GetAllAsync(null);
                var reshealthMetric = _mapper.Map<List<HealthMetricResponse>>(healthMetric);
                return new ApiResponse().SetOk(reshealthMetric);
            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }

        public async Task<ApiResponse> UpdateHealthMetric(int Id, HealthMetricUpdateRequest healthMetricRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var healthMetric = await _unitOfWork.HeathMetrics.GetAsync(c => c.Id == Id && c.Status == true);
                if (healthMetric == null)
                {
                    return apiResponse.SetNotFound("Can not found the Children's health detail");
                }
                _mapper.Map(healthMetricRequest, healthMetric);
                await _unitOfWork.SaveChangeAsync();
                return apiResponse.SetOk("Children's health details updated successfully");

            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }
    }
}
