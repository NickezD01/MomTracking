using Application.Request.Post;
using Application.Response;
using DocumentFormat.OpenXml.Spreadsheet;
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
        Task<ApiResponse> GetAllPostsAsync(int pageIndex = 1, int pageSize = 10);
        Task<ApiResponse> GetPostsByUserAsync(int accountId, int pageIndex = 1, int pageSize = 10);
        Task<ApiResponse> GetMyPostsAsync(int pageIndex = 1, int pageSize = 10);
    }
}
