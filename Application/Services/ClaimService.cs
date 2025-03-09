using System.Security.Claims;
using Application.Interface;
using Domain.Entity;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimDTO GetUserClaim()
        {
            var tokenUserId = _httpContextAccessor.HttpContext!.User.FindFirst("UserId") 
                              ?? _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier);
    
            var tokenUserRole = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Role) 
                                ?? _httpContextAccessor.HttpContext!.User.FindFirst("Role");
    
            var tokenUserName = _httpContextAccessor.HttpContext!.User.FindFirst("FullName")
                                ?? _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name);

            if (tokenUserId == null)
            {
                throw new ArgumentNullException("UserId can not be found!");
            }

            if (tokenUserRole == null)
            {
                throw new ArgumentNullException("User Role can not be found!");
            }

            var userId = Int32.Parse(tokenUserId.Value);
            Role userRole = Enum.Parse<Role>(tokenUserRole.Value);
            var fullName = tokenUserName?.Value ?? "Unknown";

            var userClaim = new ClaimDTO
            {
                Role = userRole,
                Id = userId,
                Name = fullName,
            };

            return userClaim;
        }     
       
    }
    public class ClaimDTO
    {
        public int Id { get; set; }
        public Role Role { get; set; }

        public string Name { get; set; }
    }


}
