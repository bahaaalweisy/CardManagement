using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CardManagement.Core.Models.Users;
using CardManagement.Services.Interfaces;
using CardManagementApis.Infrastructure.Middlewares;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CardManagementApis.Controllers
{
    public class BaseAuthorizeController : BaseAppController
    {
        private readonly IUserService _userService;

        public BaseAuthorizeController(IUserService userService)
        {
            this._userService = userService;
        }        
        [NonAction]
        public async Task<UserDetailModel> GetLoggedInUserAsync()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                return null; 
            }
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (jwtToken == null)
            {
                return null; 
            }
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value
                        ?? jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _userService.FindUserByEmailAsync(email);
                user.Roles = (await _userService.GetRolesAsync(user))?.ToList();
                return user;
            }
            return null;
        }


    }
}