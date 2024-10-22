using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using CardManagement.Core.Domain.Users;
using CardManagement.Core.Models.Account;
using CardManagement.Core.Models.Common;
using CardManagement.Core.Models.Users;
using CardManagement.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace CardManagementApis.Controllers
{
    public class AccountController : BaseAuthorizeController
    {
        #region Properties
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        #endregion
        #region constructor
        public AccountController(UserManager<User> userManager, IUserService userService, IConfiguration config ) : base(userService)
        {
            this._userService = userService;
            this._userManager = userManager;
            this._config = config;
        }
        #endregion

        #region Methods
        [HttpPost("token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ReturnResult))]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
          
            var response = new ReturnResult();

            try
            {
                if (ModelState.IsValid)
                {
                    var isValidLogin = await _userService.CheckUserPasswordAsync(loginModel.Email, loginModel.Password);
                    // Check if the login is valid
                    if (isValidLogin)
                    {
                        var responseWithToken = new ReturnValuedResult<TokenResponseModel>();              
                        responseWithToken.Value = await _userService.GetTokenAsync(loginModel.Email);
                        // Check if the user is active
                        var user = await _userManager.FindByEmailAsync(loginModel.Email);
                        if (user != null && user.IsActive == false)
                        {
                            response.Errors.Add("حسابك غير مفعل يرجى التواصل مع مسؤول النظام");
                            return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
                        }

                        return new ObjectResult(responseWithToken) { StatusCode = (int)HttpStatusCode.OK };
                    }
                    else
                    {
                        // Invalid login credentials
                        response.Errors.Add("الايميل او كلمة السر غير صحيحة");
                        return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
                    }
                }
                else
                {
                    // ModelState is not valid
                    response.Errors.Add("الايميل او كلمة السر غير صحيحة");
                    return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                response.Errors.Add("الايميل او كلمة السر غير صحيحة");
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }




        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ReturnValuedResult<List<string>>))]
        public async Task<IActionResult> Save([FromBody] SaveUserDetailModel userdetails)
        {
            var response = new ReturnResult();

            if (!ModelState.IsValid)
            {
                response.Errors.AddRange(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                return BadRequest(response);
            }

            var result = await _userService.CreateAsync(userdetails);
            if (result.Succeeded)
            {
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.Created };
            }

            response.Errors.AddRange(result.Errors.Select(x => x.Description));
            return BadRequest(response);
        }


        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ReturnValuedResult<List<string>>))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = new ReturnResult();
            if (id != Guid.Empty)
            {
                var iddelete = await _userService.DeleteAsync(id);
                if (iddelete != null)
                    return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
                response.Errors.Add("Unable to delete user please try again later");
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
            response.Errors.Add("Please try again.");
            return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ReturnResult))]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var userDetail = await _userManager.FindByEmailAsync(User?.Identity?.Name ?? string.Empty);
                if (userDetail == null)
                    return new ObjectResult(new ReturnResult()) { StatusCode = (int)HttpStatusCode.NotFound };
                await _userManager.RemoveAuthenticationTokenAsync(userDetail, "RealEstate", "Login");
                return new ObjectResult(new ReturnResult()) { StatusCode = (int)HttpStatusCode.OK };
            }
            return new ObjectResult(new ReturnResult()) { StatusCode = (int)HttpStatusCode.NotFound };
        }

        #endregion
    }
}