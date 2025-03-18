using System;

namespace Application.Response.Comment
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImageUrl { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public bool IsEdited { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}