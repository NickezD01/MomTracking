using Application.Interface;
using Application.Request.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPost(int postId)
        {
            var response = await _commentService.GetCommentsByPostAsync(postId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("user/{accountId}")]
        public async Task<IActionResult> GetCommentsByUser(int accountId)
        {
            var response = await _commentService.GetCommentsByUserAsync(accountId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpGet("my-comments")]
        public async Task<IActionResult> GetMyComments()
        {
            var response = await _commentService.GetMyCommentsAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            var response = await _commentService.CreateCommentAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] UpdateCommentRequest request)
        {
            var response = await _commentService.UpdateCommentAsync(commentId, request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var response = await _commentService.DeleteCommentAsync(commentId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}