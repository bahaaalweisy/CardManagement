using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using CardManagement.Core;
using CardManagement.Core.Domain.Users;
using CardManagement.Core.Models.Account;
using CardManagement.Core.Models.Users;
using CardManagement.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CardManagement.Services.Users
{
    public class UserService : IUserService
    {

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly ICommonService _commanService;
        private readonly IRepository<UserRoleAccessibility> _userRoleAccessibilityRepository;
        public UserService(UserManager<User> userManager,
            IConfiguration configuration,           
            IRepository<UserRoleAccessibility> userRoleAccessibilityRepository,
            IMapper mapper,RoleManager<UserRole> roleManager, ICommonService commanService)
        {
            this._userManager = userManager;
            this._configuration = configuration;     
            this._mapper = mapper;         
            this._roleManager = roleManager;
            this._commanService = commanService;
            this._userRoleAccessibilityRepository = userRoleAccessibilityRepository;       
        }
 
        public async Task<UserDetailModel?> FindUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentNullException();
            return this._mapper.Map<UserDetailModel>(user);
        }
        public async Task<IList<string>> GetRolesAsync(UserDetailModel user)
        {
            var userInfo = _mapper.Map<User>(user);
            return await _userManager.GetRolesAsync(userInfo);
        }
        public async Task<TokenResponseModel> GetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Log user properties for debugging
                Console.WriteLine($"User email confirmed: {user.EmailConfirmed}, User active: {user.IsActive}, User deleted: {user.IsDeleted}");

                if (user.EmailConfirmed && !user.IsDeleted && user.IsActive)
                {
                    return new TokenResponseModel() { Token = await this.GenerateToken(user) };
                }
            }
            return new TokenResponseModel() { Token = string.Empty };
        }


        private async Task<string> GenerateToken(User applicationUser)
        {
            if (applicationUser == null)
                return string.Empty;
            int tokenExpiryinMinutes = Convert.ToInt32(_configuration["Jwt:tokenExpiryinMinutes"]);
            var timestamp = DateTime.Now.AddSeconds(-1).ToFileTime();
            var expTimestamp = DateTime.Now.AddMinutes(tokenExpiryinMinutes).ToFileTime();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, timestamp.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, expTimestamp.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, applicationUser.Email));
            claims.Add(new Claim("Id", applicationUser.Id.ToString()));
            claims.Add(new Claim("UserName", $"{applicationUser.FirstName} {applicationUser.LastName}"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email));
            var userRoles = await _userManager.GetRolesAsync(applicationUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(tokenExpiryinMinutes),
            signingCredentials: signingCredentials
            );
            var tokenVal = new JwtSecurityTokenHandler().WriteToken(token);
            await _userManager.SetAuthenticationTokenAsync(applicationUser, "RealEstate", "Login", tokenVal);
            return tokenVal;
        }

        #region User CRUD

        public async Task<IdentityResult> CreateAsync(SaveUserDetailModel userdetails)
        {
            if (userdetails == null)
                throw new ArgumentNullException(nameof(userdetails));

            // Check if a user with the same email or phone number already exists
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u =>
                (u.PhoneNumber == userdetails.PhoneNumber || u.Email == userdetails.Email) && !u.IsDeleted);

            if (existingUser != null)
            {
                string errorMessage;
                if (existingUser.PhoneNumber == userdetails.PhoneNumber && existingUser.Email == userdetails.Email)
                {
                    errorMessage = $"A user already exists with the phone number {userdetails.PhoneNumber} and email {userdetails.Email}.";
                }
                else if (existingUser.PhoneNumber == userdetails.PhoneNumber)
                {
                    errorMessage = $"A user already exists with the phone number {userdetails.PhoneNumber}.";
                }
                else
                {
                    errorMessage = $"A user already exists with the email {userdetails.Email}.";
                }

                return IdentityResult.Failed(new IdentityError { Code = "400", Description = errorMessage });
            }

            // Create the user and map details
            var user = _mapper.Map<User>(userdetails);
            user.UserName = userdetails.Email;
            user.EmailConfirmed = true;
            user.IsActive = true;
            // Create the user in the database
            var status = await _userManager.CreateAsync(user, userdetails.Password);
            if (!status.Succeeded)
            {
                return status;
            }

            var roleExists = await _roleManager.RoleExistsAsync("User");
            if (!roleExists)
            {            
                var userRole = new UserRole
                {
                    Name = "User",                  
                };         
                var roleCreationStatus = await _roleManager.CreateAsync(userRole);
                if (!roleCreationStatus.Succeeded)
                {
                    return IdentityResult.Failed(new IdentityError { Code = "500", Description = "Failed to create 'User' role." });
                }
            }
            var addToRoleStatus = await _userManager.AddToRoleAsync(user, "User");
            if (!addToRoleStatus.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError { Code = "500", Description = "Failed to assign 'User' role." });
            }

            return status;
        }

        public async Task<bool> CheckUserPasswordAsync(User user, string password)
        {

            return await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task<bool> CheckUserPasswordAsync(string userName, string password)
        {
            var user = await _userManager.FindByEmailAsync(userName);

            if (user != null && !user.IsDeleted)
            {

                return await this.CheckUserPasswordAsync(user, password);
            }
            return false;
        }
        public async Task<IdentityResult> DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                user.IsDeleted = true;
                return await _userManager.UpdateAsync(user);
            }
            return IdentityResult.Failed(new IdentityError() { Code = "400", Description = "No user found" });
        }
        public IDictionary<string, object> DecodeJwtToken(string token)
        {
            var parts = token.Split('.');
            if (parts.Length < 2)
            {
                throw new ArgumentException("Invalid JWT token.");
            }
            var payload = parts[1];
            payload = payload.Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }
            var payloadJson = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(payloadJson);
        }
        #endregion
   
    }
}