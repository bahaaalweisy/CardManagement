using Microsoft.AspNetCore.Identity;

using CardManagement.Core.Domain.Users;
using CardManagement.Core.Models.Account;
using CardManagement.Core.Models.Common;
using CardManagement.Core.Models.Pagination;
using CardManagement.Core.Models.Users;

namespace CardManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<IList<string>> GetRolesAsync(UserDetailModel user);
        Task<UserDetailModel?> FindUserByEmailAsync(string email);
        Task<bool> CheckUserPasswordAsync(User user, string password);
        Task<bool> CheckUserPasswordAsync(string userName, string password);
        Task<TokenResponseModel> GetTokenAsync(string email);
     
        #region User CRUD  
        Task<IdentityResult> CreateAsync(SaveUserDetailModel userdetails);
        Task<IdentityResult> DeleteAsync(Guid id);
       
        #endregion

   
      
    }
}