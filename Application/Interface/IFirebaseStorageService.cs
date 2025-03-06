using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadUserImage(string userName, IFormFile file);
    }
}
