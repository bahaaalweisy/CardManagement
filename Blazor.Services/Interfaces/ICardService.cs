using CardManagement.Core.Domain.Activities;
using CardManagement.Core.Models.Cards;
using CardManagement.Core.Models.Common;
using CardManagement.Core.Models.Contact;
using CardManagement.Core.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardManagement.Services.Interfaces
{
    public interface ICardService
    {
        Task<PagedList<GetAllCardsModel>> GetPaginatedListAsync(PagedRequestListModel pagedRequest);
        Task<GetAllCardsModel> GetByIdAsync(Guid id);
        Task<CardApplicationModel> ApplyForCardAsync(CardApplicationModel contactAddModel);
        Task AddCreditAsync(Guid cardId, decimal amount);
        Task SetPinAsync(Guid cardId, string pin);
        Task BlockCardAsync(Guid cardId);
        Task TransferBalanceAsync(Guid fromCardId, Guid toCardId, decimal amount);
    }
}
