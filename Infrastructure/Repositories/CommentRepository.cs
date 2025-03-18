using Application.Repository;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Comment>> GetCommentsByPost(int postId, int pageIndex = 1, int pageSize = 20)
        {
            return await _db
                .Include(c => c.Account)
                .Where(c => c.PostId == postId && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByUser(int accountId, int pageIndex = 1, int pageSize = 20)
        {
            return await _db
                .Include(c => c.Post)
                .Where(c => c.AccountId == accountId && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCommentCountForPost(int postId)
        {
            return await _db.CountAsync(c => c.PostId == postId && !c.IsDeleted);
        }
    }
}
