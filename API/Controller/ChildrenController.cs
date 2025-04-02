using API.Middleware;
using Application.Interface;
using Application.Request.Children;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ChildrenController : ControllerBase
    {
        public IChildenService _childrenService;
        public ChildrenController(IChildenService childrenService)
        {
            _childrenService = childrenService;
        }

        //[HttpPost("AddNewChildren")]


        
        [Authorize(Roles = "Customer")]
        [RequirePaidStatusAttribute]
        [Route("AddNewChildren")]
        [HttpPost]

        public async Task<IActionResult> AddNewChildren(ChildrenRequest childrentRequest)
        {

            var result = await _childrenService.AddNewChildren(childrentRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Customer")]
        //[RequirePaidStatusAttribute]
        [HttpGet("GetAllChildren")]
        public async Task<IActionResult> GetAllChildren()
        {
            var response = await _childrenService.GetAllChildren();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Customer")]
        [RequirePaidStatusAttribute]
        [HttpDelete("DeleteChildrenDetail/{id}")]
        public async Task<IActionResult> DeleteChildrenDetail(int id)
        {
            var response = await _childrenService.DeleteChildrenData(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Customer")]
        [RequirePaidStatusAttribute]
        [HttpPut("UpdateChildrenData/{id}")]
        public async Task<IActionResult> UpdateChildrenData(int id, ChildrenUpdateRequest childrentRequest)
        {
            var resposne = await _childrenService.UpdateChildrenData(id,childrentRequest);
            return resposne.IsSuccess ? Ok(resposne) : BadRequest(resposne);
        }
    }
}
