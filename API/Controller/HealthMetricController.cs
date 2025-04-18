﻿using API.Middleware;
using Application.Interface;
using Application.Request.Children;
using Application.Request.HealthMetric;
using Application.Services;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthMetricController : ControllerBase
    {
        public IHeathMetricService _heathMetricService;

        public HealthMetricController(IHeathMetricService heathMetricService)
        {
            _heathMetricService = heathMetricService;
        }

        [Authorize(Roles = "Customer")]
        [RequirePaidStatusAttribute]
        [HttpPost("AddNewHealthMetric")]
        public async Task<IActionResult> AddNewHealthMetric (HealthMetricRequest healthMetricRequest)
        {
            var result = await _heathMetricService.AddHealthMetric(healthMetricRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Customer")]
        //[RequirePaidStatusAttribute]
        [HttpGet("GetAllHealthMetric")]
        public async Task<IActionResult> GetAllHealthMetric()
        {
            var response = await _heathMetricService.GetAllHealthMetric();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Customer")]
        [RequirePaidStatusAttribute]
        [HttpDelete("DeleteHealthMetric/{id}")]
        public async Task<IActionResult> DeleteHealthMetric(int id)
        {
            var response = await _heathMetricService.DeleteHealthMetric(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Customer")]
        [RequirePaidStatusAttribute]
        [HttpPut("UpdateHealthMetric/{id}")]
        public async Task<IActionResult> UpdateHealthMetric(int id, HealthMetricUpdateRequest healthMetricRequest)
        {
            var resposne = await _heathMetricService.UpdateHealthMetric(id, healthMetricRequest);
            return resposne.IsSuccess ? Ok(resposne) : BadRequest(resposne);
        }

        [Authorize(Roles = "Customer")]
        [RequirePaidStatusAttribute]
        [HttpGet("CompareHealthMetricData/{id}")]
        public async Task<IActionResult> CompareData(int id)
        {
            var result = await _heathMetricService.CompareData(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
    

