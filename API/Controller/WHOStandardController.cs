using Application.Interface;
using Application.Request.WHO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class WHOStandardController : ControllerBase
    {
        public IWHOStandardService _standard;
        public WHOStandardController(IWHOStandardService standard)
        {
            _standard = standard;
        }
        [HttpPost("AddWHOStandard")]
        public async Task<IActionResult> AddWHOStandard(StandardRequest standardRequest)
        {
            var result = await _standard.AddStandard(standardRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("GetAllWHOStatistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var response = await _standard.GetAllStandard();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        [HttpDelete("DeleteWHOData/{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            var response = await _standard.DeleteStandard(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        [HttpPut("UpdateWHOData/{id}")]
        public async Task<IActionResult> UpdateData(int id, StandardRequest standardRequest)
        {
            var response = await _standard.UpdateStandard(id, standardRequest);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
