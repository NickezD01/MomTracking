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
                healthMetric.ChildrentId = healthMetricRequest.ChildrentId;
                var DetailsExist = await _unitOfWork.HeathMetrics.GetAsync(x => x.PregnancyWeek == healthMetric.PregnancyWeek);
                //Bo sung tinh tuan thai
                var children = await _unitOfWork.Childrens.GetAsync(c => c.Id == healthMetric.ChildrentId);
                DateTime today = DateTime.Now;
                TimeSpan timeUntilDue = children.Birth - today;
                int w = 0;
                if (DetailsExist == null || DetailsExist.ChildrentId != healthMetric.ChildrentId)
                {
                        await _unitOfWork.HeathMetrics.AddAsync(healthMetric);
                        w = (int)(timeUntilDue.TotalDays / 7);
                        healthMetric.PregnancyWeek = 40 - w;
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
                if (healthMetric.Weight < standard.WeightMin)
                {
                        warnings.Add("WARNING: Thai nhi có nguy cơ bị suy dinh dưỡng, mẹ cần quan tâm nhiều hơn đến sức khỏe và bồi bổ cho thai nhi!!!");
                }
                if (healthMetric.Weight > standard.WeightMax)
                {
                    warnings.Add("WARNING: Thai nhi có nguy cơ béo phì, mẹ cần chú ý chế độ ăn uống để tránh trường hợp xấu đến với thai nhi!!!");
                }
                if (healthMetric.Lenght < standard.LenghtMin  && healthMetric.Weight < standard.WeightMin)
                {
                    warnings.Add("WARNING: Thai nhi bị suy dinh dưỡng, mẹ chú ý bồi bổ cho bé và đến khám ở cơ sở y tế hoặc bệnh viện gần nhất!!!");
                }
                if (healthMetric.Lenght > standard.LenghtMin && healthMetric.Weight > standard.WeightMin)
                {
                    warnings.Add("WARNING: Thai nhi bị béo phì, mẹ cần chú ý chế độ ăn uống của mình sao cho phù hợp và đến khám ở cơ sở y tế hoặc bệnh viện gần nhất!!!");
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

                return apiResponse.SetOk("Happy : Tình trạng thai nhi rất tốt tiếp tục theo dõi sức khỏe bé mẹ nhé!!!");

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
