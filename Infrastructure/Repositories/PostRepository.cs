using Application.Repository;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Post>> GetPostsWithComments()
        {
            return await _db
                .Include(p => p.Account)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Account)
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedDate)

                .ToListAsync();
        }

        public async Task<Post> GetPostWithComments(int postId)
        {
            return await _db
                .Include(p => p.Account)
                .Include(p => p.Comments.Where(c => !c.IsDeleted))
                    .ThenInclude(c => c.Account)
                .FirstOrDefaultAsync(p => p.Id == postId && !p.IsDeleted);
        }

    
       

      

        public async Task<List<Post>> GetPostsByUser(int accountId)
        {
            return await _db
                .Include(p => p.Account)
                .Include(p => p.Comments.Where(c => !c.IsDeleted))
                .Where(p => p.AccountId == accountId && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedDate)

                .ToListAsync();
        }

        public async Task<int> GetTotalPostsCount()
        {
            return await _db.CountAsync(p => !p.IsDeleted);
        }
    }
}
