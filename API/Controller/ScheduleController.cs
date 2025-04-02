using API.Middleware;
using Application.Interface;
using Application.Request.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        public IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [Authorize(Roles = "Customer")]
        [RequirePaidStatusAttribute]
        [HttpPost("CreateSchedule")]
        public async Task<IActionResult> CreateSchedule(ScheduleRequest scheduleRequest)
        {
            var result = await _scheduleService.CreateSchedule(scheduleRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Customer")]
        [RequirePaidStatusAttribute]
        [HttpGet("GetAllSchedule")]
        public async Task<IActionResult> GetAllSchedule()
        {
            var response = await _scheduleService.GetAllSchedule();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Customer")]
        [RequirePaidStatusAttribute]
        [HttpDelete("DeleteSchedule/{id}")]
        public  async Task<IActionResult> DeleteSchedule(int id)
        {
            var response = await _scheduleService.DeleteSchedule(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Customer")]
        [RequirePaidStatusAttribute]
        [HttpPut("UpdateSchedule/{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, ScheduleRequest scheduleRequest)
        {
            var response = await _scheduleService.UpdateSchedule(id, scheduleRequest);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
