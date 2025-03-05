using Application.Request.HealthMetric;
using Application.Request.WHO;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IWHOStandardService
    {
        Task<ApiResponse> AddStandard(StandardRequest standardRequest);
        Task<ApiResponse> GetAllStandard();
        Task<ApiResponse> DeleteStandard(int Id);
        Task<ApiResponse> UpdateStandard(int Id, StandardRequest standardRequest);
    }
}
