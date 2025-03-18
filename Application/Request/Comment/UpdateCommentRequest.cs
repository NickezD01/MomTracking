using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Request.Comment
{
    public class UpdateCommentRequest
    {
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Content { get; set; }
    }
}