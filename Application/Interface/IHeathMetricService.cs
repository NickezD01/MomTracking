using Application.Request.Children;
using Application.Request.HealthMetric;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IHeathMetricService
    {
        Task<ApiResponse> AddHealthMetric(HealthMetricRequest healthMetricRequest);
        Task<ApiResponse> GetAllHealthMetric();
        Task<ApiResponse> DeleteHealthMetric(int Id);
        Task<ApiResponse> UpdateHealthMetric(HealthMetricUpdateRequest healthMetricRequest);
        
    }
}
