using Application.Response.Comment;
using System;
using System.Collections.Generic;

namespace Application.Response.Post
{
    public class PostResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; } // URL hình ảnh
        public bool IsEdited { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int AccountId { get; set; }
        public string AuthorName { get; set; }
        public string? AuthorImageUrl { get; set; }
        public int CommentCount { get; set; }
        public List<CommentResponse>? Comments { get; set; }
    }
}