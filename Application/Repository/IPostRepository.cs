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
        Task<List<Post>> GetPostsWithComments();
        Task<Post> GetPostWithComments(int postId);
        Task<List<Post>> GetPostsByUser(int accountId);
        Task<int> GetTotalPostsCount();
    }
}
