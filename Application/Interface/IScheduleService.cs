using Application.Request.HealthMetric;
using Application.Request.Schedule;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IScheduleService
    {
        Task<ApiResponse> CreateSchedule(ScheduleRequest scheduleRequest);
        Task<ApiResponse> GetAllSchedule();
        Task<ApiResponse> DeleteSchedule(int Id);
        Task<ApiResponse> UpdateSchedule(int Id, ScheduleRequest scheduleRequest);
    }
}
