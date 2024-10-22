using AutoMapper;
using CardManagement.Core.Models.Common;
using CardManagement.Core.Models.Pagination;
using CardManagement.Core;
using CardManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CardManagement.Core.Domain.Activities;
using CardManagement.Core.Models.Cards;
using System.Net.Http;
using CardManagement.Core.Domain.Common;
using CardManagement.Core.Models.Contact;
using CardManagement.Services.Common;

namespace CardManagement.Services.Cards
{
    public class CardService : ICardService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Card> _cardRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICommonService _commanService;
        public CardService(IMapper mapper, IRepository<Card> cardRepository, IHttpClientFactory httpClientFactory, ICommonService commanService)
        {
            _mapper = mapper;
            _cardRepository = cardRepository;
            _httpClientFactory = httpClientFactory;
            _commanService = commanService;

        }

        public async Task<PagedList<GetAllCardsModel>> GetPaginatedListAsync(PagedRequestListModel pagedRequest)
        {
            if (pagedRequest == null)
                throw new ArgumentNullException(nameof(pagedRequest));

            var cardQuery = _cardRepository.FindAllAsQueryable(x => !x.IsDeleted);
            if (!string.IsNullOrEmpty(pagedRequest.SearchText))
            {
                cardQuery = cardQuery.Where(a => a.CardNumber.Contains(pagedRequest.SearchText));
            }

            var cards = await cardQuery.ToListAsync();
            if (cards.Count == 0)
                throw new Exception("Data Not Found");

            var list = _mapper.Map<List<GetAllCardsModel>>(cards).AsQueryable();
            return list.ToPaginatedList(pagedRequest.PageNumber, pagedRequest.PageSize);
        }

        public async Task<GetAllCardsModel> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var card = await _cardRepository
                .FindAllAsQueryable(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (card == null)
                return null;

            return _mapper.Map<GetAllCardsModel>(card);
        }

        public async Task<CardApplicationModel> ApplyForCardAsync(CardApplicationModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            string photoPath = null;

            // Handle file upload for ID photo
            if (model.IdPhoto != null && model.IdPhoto.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.IdPhoto.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    photoPath = await _commanService.UploadAttachmentAsync(model.IdPhoto.FileName, imageBytes);

                    if (string.IsNullOrEmpty(photoPath))
                    {
                        throw new InvalidOperationException("Failed to upload the ID photo.");
                    }
                }
            }

          
            var cardEntity = _mapper.Map<Card>(model);
            cardEntity.CreatedOnUtc = DateTime.UtcNow;
            cardEntity.UpdatedOnUtc = DateTime.UtcNow;
            cardEntity.CardNumber = _commanService.GenerateCardNumber();
            cardEntity.ExpiryDate = DateTime.Now.AddYears(5);
            cardEntity.CVV = _commanService.GenerateCVV();
            cardEntity.Balance = 0;
            cardEntity.Status = CardStatus.Active;
            cardEntity.UserId = model.UserId;
            cardEntity.PIN = _commanService.GeneratePin();
            cardEntity.PhotoPath = photoPath;

          
            await _cardRepository.InsertAsync(cardEntity);

        
            var createdCardModel = _mapper.Map<CardApplicationModel>(cardEntity);
            return createdCardModel;
        }

        public async Task AddCreditAsync(Guid cardId, decimal amount)
        {
            var card = await _cardRepository.GetByIdAsync(cardId);
            if (card == null)
                throw new Exception("Card not found");

            card.Balance += amount;
            await _cardRepository.UpdateAsync(card);
        }

        public async Task SetPinAsync(Guid cardId, string pin)
        {
            var card = await _cardRepository.GetByIdAsync(cardId);
            if (card == null)
                throw new Exception("Card not found");

            card.PIN = pin;
            await _cardRepository.UpdateAsync(card);
        }

        public async Task BlockCardAsync(Guid cardId)
        {
            var card = await _cardRepository.GetByIdAsync(cardId);
            if (card == null)
                throw new Exception("Card not found");

            card.Status = CardStatus.Blocked;
            await _cardRepository.UpdateAsync(card);
        }

        public async Task TransferBalanceAsync(Guid fromCardId, Guid toCardId, decimal amount)
        {
            var fromCard = await _cardRepository.GetByIdAsync(fromCardId);
            var toCard = await _cardRepository.GetByIdAsync(toCardId);

            if (fromCard == null || toCard == null)
                throw new Exception("One or both cards not found");

            if (fromCard.Balance < amount)
                throw new Exception("Insufficient balance");

            fromCard.Balance -= amount;
            toCard.Balance += amount;

            await _cardRepository.UpdateAsync(fromCard);
            await _cardRepository.UpdateAsync(toCard);
        }
        private async Task<string> CallFakeExternalApi(CardApplicationModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://fakeapi.com/applyCard");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
