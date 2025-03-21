using Application.Request.Comment;
using Application.Response;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ICommentService
    {
        Task<ApiResponse> CreateCommentAsync(CreateCommentRequest request);
        Task<ApiResponse> UpdateCommentAsync(int commentId, UpdateCommentRequest request);
        Task<ApiResponse> DeleteCommentAsync(int commentId);
        Task<ApiResponse> GetCommentsByPostAsync(int postId);
        Task<ApiResponse> GetCommentsByUserAsync(int accountId);
        Task<ApiResponse> GetMyCommentsAsync();
    }
}
