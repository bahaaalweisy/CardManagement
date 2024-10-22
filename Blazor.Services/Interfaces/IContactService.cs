using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardManagement.Core.Models.Common;
using CardManagement.Core.Models.Contact;
using CardManagement.Core.Models.Pagination;
using System.Threading.Tasks;

namespace CardManagement.Services.Interfaces
{
    public interface IContactService
    {
        Task<ContactAddModel> CreateAsync(ContactAddModel contactAddModel);
        Task<GetAllContactModel> GetByIdAsync(Guid id);
        Task<ContactUpdateModal> UpdateAsync(ContactUpdateModal contactUpdateModal);
        Task<bool> DeleteAsync(Guid id);
        Task<PagedList<GetAllContactModel>> GetPaginatedListAsync(PagedRequestListModel pagedRequest);
    }
}
