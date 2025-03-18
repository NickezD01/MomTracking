using Domain.Entity;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Repository
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetPostsWithComments(int pageIndex = 1, int pageSize = 10);
        Task<Post> GetPostWithComments(int postId);
        Task<List<Post>> GetPostsByUser(int accountId, int pageIndex = 1, int pageSize = 10);
        Task<int> GetTotalPostsCount();
    }
}
