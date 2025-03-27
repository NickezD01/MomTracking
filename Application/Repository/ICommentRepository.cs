using Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repository
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetCommentsByPost(int postId, int pageIndex = 1, int pageSize = 20);
        Task<List<Comment>> GetCommentsByUser(int accountId, int pageIndex = 1, int pageSize = 20);
        Task<int> GetCommentCountForPost(int postId);
    }
}
