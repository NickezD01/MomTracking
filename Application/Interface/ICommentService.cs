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
        Task<ApiResponse> GetCommentsByPostAsync(int postId, int pageIndex = 1, int pageSize = 20);
        Task<ApiResponse> GetCommentsByUserAsync(int accountId, int pageIndex = 1, int pageSize = 20);
        Task<ApiResponse> GetMyCommentsAsync(int pageIndex = 1, int pageSize = 20);
    }
}
