using CardManagement.Core.Models.Cards;
using CardManagement.Core.Models.Common;
using CardManagement.Core.Models.Pagination;
using CardManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Linq;
using System;
using System.Threading.Tasks;
using CardManagement.Core.Domain.Activities;
using CardManagement.Services.Common;

namespace CardManagementApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : BaseAuthorizeController
    {
        private readonly ICardService _cardService;
        private readonly ICommonService _commanService;
        public CardController(ICardService cardService, IUserService userService, ICommonService commanService) : base(userService)
        {
            _cardService = cardService;
            _commanService = commanService;
        }

        #region Methods
        [HttpPost("apply")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status304NotModified, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ReturnResult))]
        public async Task<IActionResult> ApplyForCard([FromForm] CardApplicationModel model)
        {
            var response = new ReturnResult();
            var currentUser = await base.GetLoggedInUserAsync();
            if (currentUser == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(new ReturnResult { Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList() });

            try
            {
                model.UserId = currentUser.Id;
                var card = await _cardService.ApplyForCardAsync(model);
                if (card != null)
                    return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.Created };

                response.Errors.Add("Unable to insert Card. Please try again later.");
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        [HttpPost("buy-credit/{cardId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ReturnResult))]
        public async Task<IActionResult> BuyCredit(Guid cardId, [FromBody] decimal amount)
        {
            var response = new ReturnResult();
            var currentUser = await base.GetLoggedInUserAsync();
            if (currentUser == null)
                return Unauthorized();

            try
            {
                await _cardService.AddCreditAsync(cardId, amount);
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }

        [HttpPost("set-pin/{cardId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ReturnResult))]
        public async Task<IActionResult> SetPin(Guid cardId, [FromBody] string pin)
        {
            var response = new ReturnResult();
            var currentUser = await base.GetLoggedInUserAsync();
            if (currentUser == null)
                return Unauthorized();

            try
            {
                await _cardService.SetPinAsync(cardId, pin);
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }

        [HttpPost("block-card/{cardId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ReturnResult))]
        public async Task<IActionResult> BlockCard(Guid cardId)
        {
            var response = new ReturnResult();
            var currentUser = await base.GetLoggedInUserAsync();
            if (currentUser == null)
                return Unauthorized();

            try
            {
                await _cardService.BlockCardAsync(cardId);
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }

        [HttpPost("transfer-balance")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ReturnResult))]
        public async Task<IActionResult> TransferBalance([FromBody] TransferBalanceModel model)
        {
            var response = new ReturnResult();
            var currentUser = await base.GetLoggedInUserAsync();
            if (currentUser == null)
                return Unauthorized();

            try
            {
                await _cardService.TransferBalanceAsync(model.FromCardId, model.ToCardId, model.Amount);
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnValuedResult<GetAllCardsModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ReturnResult))]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = new ReturnValuedResult<GetAllCardsModel>();
            var currentUser = await base.GetLoggedInUserAsync();
            if (currentUser == null)
                return Unauthorized();

            try
            {
                var card = await _cardService.GetByIdAsync(id);
                if (card == null)
                {
                    response.Errors.Add("Card not found");
                    return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.NotFound };
                }
                response.Value = card;
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        #endregion
    }
}
