using Application.Interface;
using Application.Request.Comment;
using Application.Response;
using Application.Response.Comment;
using AutoMapper;
using Domain.Entity;
using Domain; // Add this to access the Role enum
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, IClaimService claimService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimService = claimService;
        }

        public async Task<ApiResponse> CreateCommentAsync(CreateCommentRequest request)
        {
            try
            {
                var userClaim = _claimService.GetUserClaim();
                
                // Check if post exists
                var post = await _unitOfWork.Posts.GetAsync(p => p.Id == request.PostId && !p.IsDeleted);
                if (post == null)
                    return new ApiResponse().SetNotFound("Post not found");

                var comment = new Comment
                {
                    AccountId = userClaim.Id,
                    PostId = request.PostId,
                    Content = request.Content,
                    IsEdited = false,
                    LastUpdateTime = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Comments.AddAsync(comment);
                await _unitOfWork.SaveChangeAsync();

                var commentResponse = _mapper.Map<CommentResponse>(comment);
                return new ApiResponse().SetOk(commentResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error creating comment: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateCommentAsync(int commentId, UpdateCommentRequest request)
        {
            try
            {
                var userClaim = _claimService.GetUserClaim();
                
                var comment = await _unitOfWork.Comments.GetAsync(c => c.Id == commentId && !c.IsDeleted);
                if (comment == null)
                    return new ApiResponse().SetNotFound("Comment not found");

                if (comment.AccountId != userClaim.Id)
                    return new ApiResponse().SetBadRequest("You can only update your own comments");

                comment.Content = request.Content;
                comment.IsEdited = true;
                comment.LastUpdateTime = DateTime.UtcNow;
                comment.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();

                var commentResponse = _mapper.Map<CommentResponse>(comment);
                return new ApiResponse().SetOk(commentResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error updating comment: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteCommentAsync(int commentId)
        {
            try
            {
                var userClaim = _claimService.GetUserClaim();
                
                var comment = await _unitOfWork.Comments.GetAsync(c => c.Id == commentId && !c.IsDeleted);
                if (comment == null)
                    return new ApiResponse().SetNotFound("Comment not found");

                // Fix: Compare with Role enum instead of string
                if (comment.AccountId != userClaim.Id && userClaim.Role != Role.Manager)
                    return new ApiResponse().SetBadRequest("You can only delete your own comments");

                comment.IsDeleted = true;
                comment.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangeAsync();

                return new ApiResponse().SetOk("Comment deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error deleting comment: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetCommentsByPostAsync(int postId, int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                // Check if post exists
                var post = await _unitOfWork.Posts.GetAsync(p => p.Id == postId && !p.IsDeleted);
                if (post == null)
                    return new ApiResponse().SetNotFound("Post not found");

                var comments = await _unitOfWork.Comments.GetCommentsByPost(postId, pageIndex, pageSize);
                var commentResponses = _mapper.Map<List<CommentResponse>>(comments);
                return new ApiResponse().SetOk(commentResponses);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving comments: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetCommentsByUserAsync(int accountId, int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                var comments = await _unitOfWork.Comments.GetCommentsByUser(accountId, pageIndex, pageSize);
                var commentResponses = _mapper.Map<List<CommentResponse>>(comments);
                return new ApiResponse().SetOk(commentResponses);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving user comments: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetMyCommentsAsync(int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                var userClaim = _claimService.GetUserClaim();
                return await GetCommentsByUserAsync(userClaim.Id, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest($"Error retrieving your comments: {ex.Message}");
            }
        }
    }
}