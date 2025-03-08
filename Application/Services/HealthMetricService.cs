using Application.Interface;
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
                healthMetric.ChildrentId = healthMetricRequest.childrenId;
                var DetailsExist = await _unitOfWork.HeathMetrics.GetAsync(x => x.PregnancyWeek == healthMetric.PregnancyWeek);
                if (DetailsExist == null)
                {
                    if(healthMetric.PregnancyWeek > 3 && healthMetric.PregnancyWeek < 43)
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

<<<<<<< HEAD
        public Task<ApiResponse> CompareData(int Id, HealthMetricUpdateRequest healthMetricRequest)
        {
            throw new NotImplementedException();
=======
        public async Task<ApiResponse> CompareData(int Id)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {

                var healthMetric = await _unitOfWork.HeathMetrics.GetAsync(c => c.Id == Id && c.Status == true);
                var standard = await _unitOfWork.WHOStandard.GetAsync(s => s.PregnancyWeek == healthMetric.PregnancyWeek);
                if (standard == null)
                {
                    return apiResponse.SetNotFound("The pregnancy is still in its development cycle and there is no specific data yet!");
                }
                if (healthMetric == null)
                {
                    return apiResponse.SetNotFound("Can not found the Children's health detail");
                }
                List<string> warnings = new List<string>();
                if (healthMetric.PregnancyWeek >= 4 && healthMetric.PregnancyWeek <= 20)
                {
                    if(healthMetric.SacDiameter < standard.SacDiameter || healthMetric.SacDiameter > standard.SacDiameter)
                    {
                        warnings.Add("WARNING: Diameter of fetal sac is different from the standard index!!!");
                    }
                }
                if (healthMetric.PregnancyWeek >= 12 && healthMetric.PregnancyWeek <= 20)
                {
                    if(healthMetric.HeadCircumference < standard.HeadCircumference || healthMetric.HeadCircumference > standard.HeadCircumference)
                    {
                        warnings.Add("WARNING: The fetal head circumference is different from the standard index!!!");
                    }
                }
                if (healthMetric.PregnancyWeek >= 8 && healthMetric.PregnancyWeek <= 42)
                {
                    if (healthMetric.Weight < standard.Weight || healthMetric.Weight > standard.Weight)
                    {
                        warnings.Add("WARNING: The fetal weight is different from the standard index!!!");
                    }
                    if (healthMetric.Lenght < standard.Lenght || healthMetric.Lenght > standard.Lenght)
                    {
                        warnings.Add("WARNING: The fetal lenght is different from the standard index!!!");
                    }
                }
                if (healthMetric.PregnancyWeek >= 7 && healthMetric.PregnancyWeek < 9)
                { 
                    if(healthMetric.HearRate < 140 || healthMetric.HearRate > 170)
                    {
                        warnings.Add("WARNING: The fetal heart rate is different from the standard index!!!");
                    }
                }
                if (healthMetric.PregnancyWeek >= 9)
                {
                    if (healthMetric.HearRate < standard.HearRateMin || healthMetric.HearRate > standard.HearRateMax)
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
>>>>>>> CompareData
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
                healthMetric.Status = false;
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
                var healthMetric = await _unitOfWork.HeathMetrics.GetAllAsync(a => a.Status == true);
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
