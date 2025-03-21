using Application.Request.Post;
using Application.Response;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IPostService
    {
        Task<ApiResponse> CreatePostAsync(CreatePostRequest request);
        Task<ApiResponse> UpdatePostAsync(int postId, UpdatePostRequest request);
        Task<ApiResponse> DeletePostAsync(int postId);
        Task<ApiResponse> GetPostByIdAsync(int postId);
        Task<ApiResponse> GetAllPostsAsync();
        Task<ApiResponse> GetPostsByUserAsync(int accountId);
        Task<ApiResponse> GetMyPostsAsync();
    }
}
