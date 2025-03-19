using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Request.Post
{
    public class CreatePostRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(5000, MinimumLength = 10)]
        public string Content { get; set; }
        
        // Thêm trường cho hình ảnh, không bắt buộc
        public IFormFile? Image { get; set; }
    }
}