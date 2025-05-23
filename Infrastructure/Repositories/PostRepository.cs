﻿using Application.Repository;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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

        public async Task<List<Post>> GetPostsWithComments(int pageIndex = 1, int pageSize = 10)
        {
            return await _db
                .Include(p => p.Account)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Account)
                .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
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

    
       

      

        public async Task<List<Post>> GetPostsByUser(int accountId, int pageIndex = 1, int pageSize = 10)
        {
            return await _db
                .Include(p => p.Account)
                .Include(p => p.Comments.Where(c => !c.IsDeleted))
                .Where(p => p.AccountId == accountId && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalPostsCount()
        {
            return await _db.CountAsync(p => !p.IsDeleted);
        }
    }
}
