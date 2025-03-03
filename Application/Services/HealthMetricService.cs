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
                    await _unitOfWork.HeathMetrics.AddAsync(healthMetric);
                    await _unitOfWork.SaveChangeAsync();
                    return apiResponse.SetOk("Children's health details added successfully!");
                }
                return apiResponse.SetBadRequest("You have entered this week's data");
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

        public async Task<ApiResponse> UpdateHealthMetric(HealthMetricUpdateRequest healthMetricRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var healthMetric = await _unitOfWork.HeathMetrics.GetAsync(c => c.Id == healthMetricRequest.Id && c.Status == true);
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
