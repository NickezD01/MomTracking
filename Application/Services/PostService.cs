using Application.Interface;
using Application.Request.Post;
using Application.Response;
using Application.Response.Post;
using AutoMapper;
using Domain.Entity;
using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;
        public PostService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IClaimService claimService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimService = claimService;
        }

        public async Task<ApiResponse> CreatePostAsync(CreatePostRequest request)
        {
            try
            {
                var userClaim = _claimService.GetUserClaim();
                
                var post = new Post
                {
                    AccountId = userClaim.Id,
                    Title = request.Title,
                    Content = request.Content,
                    ImageUrl = request.ImageUrl, // Lưu URL hình ảnh từ request
                    IsEdited = false,
                    LastUpdateTime = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Posts.AddAsync(post);
                await _unitOfWork.SaveChangeAsync();

                var postResponse = _mapper.Map<PostResponse>(post);
                return new ApiResponse().SetOk(postResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error creating post: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdatePostAsync(int postId, UpdatePostRequest request)
        {
            try
            {
                var userClaim = _claimService.GetUserClaim();
                
                var post = await _unitOfWork.Posts.GetAsync(p => p.Id == postId && !p.IsDeleted);
                if (post == null)
                    return new ApiResponse().SetNotFound("Post not found");

                if (post.AccountId != userClaim.Id)
                    return new ApiResponse().SetBadRequest("You can only update your own posts");

                post.Title = request.Title;
                post.Content = request.Content;
                post.ImageUrl = request.ImageUrl; // Cập nhật URL hình ảnh từ request
                post.IsEdited = true;
                post.LastUpdateTime = DateTime.UtcNow;
                post.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();

                var postResponse = _mapper.Map<PostResponse>(post);
                return new ApiResponse().SetOk(postResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error updating post: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeletePostAsync(int postId)
        {
            try
            {
                var userClaim = _claimService.GetUserClaim();
                
                var post = await _unitOfWork.Posts.GetAsync(p => p.Id == postId && !p.IsDeleted);
                if (post == null)
                    return new ApiResponse().SetNotFound("Post not found");

                if (post.AccountId != userClaim.Id && userClaim.Role != Role.Manager)
                    return new ApiResponse().SetBadRequest("You can only delete your own posts");

                post.IsDeleted = true;
                post.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();

                return new ApiResponse().SetOk("Post deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error deleting post: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetPostByIdAsync(int postId)
        {
            try
            {
                var post = await _unitOfWork.Posts.GetPostWithComments(postId);
                if (post == null)
                    return new ApiResponse().SetNotFound("Post not found");

                var postResponse = _mapper.Map<PostResponse>(post);
                return new ApiResponse().SetOk(postResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving post: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetAllPostsAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var posts = await _unitOfWork.Posts.GetPostsWithComments(pageIndex, pageSize);
                var totalCount = await _unitOfWork.Posts.GetTotalPostsCount();
                
                var postResponses = _mapper.Map<List<PostResponse>>(posts);
                
                var paginatedResponse = new
                {
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    Items = postResponses
                };
                
                return new ApiResponse().SetOk(paginatedResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving posts: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetPostsByUserAsync(int accountId, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var posts = await _unitOfWork.Posts.GetPostsByUser(accountId, pageIndex, pageSize);
                var postResponses = _mapper.Map<List<PostResponse>>(posts);
                return new ApiResponse().SetOk(postResponses);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving user posts: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetMyPostsAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var userClaim = _claimService.GetUserClaim();
                return await GetPostsByUserAsync(userClaim.Id, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving your posts: {ex.Message}");
            }
        }

    }
}
