using Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repository
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetCommentsByPost(int postId);
        Task<List<Comment>> GetCommentsByUser(int accountId);
        Task<int> GetCommentCountForPost(int postId);
    }
}
