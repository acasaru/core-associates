using Interview.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace Interview.Infrastructure.Authentication
{
    public class AutheticatedUserService : IAutheticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AutheticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public int GetUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;

            var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return Convert.ToInt32(userId);
        }

        public string GetUserRole()
        {
            var user = _httpContextAccessor.HttpContext.User;

            var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

            return userRole;
        }

        
    }
}
