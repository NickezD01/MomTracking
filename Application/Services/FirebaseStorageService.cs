using Application.Interface;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly IConfiguration _config;

        public FirebaseStorageService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> UploadUserImage(string userName, IFormFile file)
        {
            string firebaseBucket = _config["Firebase:Bucket"];

            var firebaseStorage = new FirebaseStorage(firebaseBucket);

            string fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(file.FileName)}";

            if (userName.EndsWith("/"))
            {
                userName = userName.TrimEnd('/');
            }

            fileName = fileName.Replace("/", "-");

            var task = firebaseStorage.Child("UserAccounts").Child(userName).Child(fileName);

            var stream = file.OpenReadStream();
            await task.PutAsync(stream);

            return await task.GetDownloadUrlAsync();
        }
        
        public async Task<string> UploadPostImage(int postId, IFormFile file)
        {
            string firebaseBucket = _config["Firebase:Bucket"];

            var firebaseStorage = new FirebaseStorage(firebaseBucket);

            string fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(file.FileName)}";
            fileName = fileName.Replace("/", "-");

            // Lưu hình ảnh trong thư mục Posts/[postId]
            var task = firebaseStorage.Child("Posts").Child(postId.ToString()).Child(fileName);

            var stream = file.OpenReadStream();
            await task.PutAsync(stream);

            return await task.GetDownloadUrlAsync();
        }
    }
}
