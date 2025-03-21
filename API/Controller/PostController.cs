using Application.Interface;
using Application.Request.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var response = await _postService.GetAllPostsAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            var response = await _postService.GetPostByIdAsync(postId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("user/{accountId}")]
        public async Task<IActionResult> GetPostsByUser(int accountId)
        {
            var response = await _postService.GetPostsByUserAsync(accountId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpGet("my-posts")]
        public async Task<IActionResult> GetMyPosts()
        {
            var response = await _postService.GetMyPostsAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            var response = await _postService.CreatePostAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] UpdatePostRequest request)
        {
            var response = await _postService.UpdatePostAsync(postId, request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var response = await _postService.DeletePostAsync(postId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}