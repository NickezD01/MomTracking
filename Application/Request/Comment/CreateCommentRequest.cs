using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Request.Comment
{
    public class CreateCommentRequest
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Content { get; set; }
    }
}