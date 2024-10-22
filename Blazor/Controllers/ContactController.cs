using CardManagement.Core.Models.Common;
using CardManagement.Core.Models.Contact;
using CardManagement.Core.Models.Pagination;
using CardManagement.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CardManagementApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        #region Properties
        private readonly IContactService _ContactService;

        #endregion

        #region Constructor
        public ContactController(IContactService ContactService)        
        {
            _ContactService = ContactService;
        }
        #endregion

        #region Methods

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnValuedResult<PagedList<GetAllContactModel>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ReturnResult))]
        public async Task<IActionResult> List([FromQuery] PagedRequestListModel pagedRequest)
        {
            var response = new ReturnValuedResult<PagedList<GetAllContactModel>>();
            response.Value = await _ContactService.GetPaginatedListAsync(pagedRequest);
            if (response.Value == null)
                return new ObjectResult(new ReturnResult()) { StatusCode = (int)HttpStatusCode.NotFound };
            return new ObjectResult(new { Data = response, PagingParams = response.Value.GetPagingMetaData() }) { StatusCode = (int)HttpStatusCode.OK };
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnValuedResult<GetAllContactModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ReturnResult))]
        public async Task<IActionResult> View(Guid id)
        {
            var response = new ReturnValuedResult<GetAllContactModel>();
            var responseWithoutValue = new ReturnResult();
            if (id != Guid.Empty)
            {
                response.Value = await _ContactService.GetByIdAsync(id);
                if (response.Value != null)
                    return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
                responseWithoutValue.Errors.Add("Contact Not Found.");
                return new ObjectResult(responseWithoutValue) { StatusCode = (int)HttpStatusCode.NotFound };
            }
            responseWithoutValue.Errors.Add("Invalid ID provided.");
            return new ObjectResult(responseWithoutValue) { StatusCode = (int)HttpStatusCode.BadRequest };
        }

        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status304NotModified, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ReturnResult))]
        public async Task<IActionResult> Save(ContactAddModel contactAddModel)
        {
            var response = new ReturnResult();
            if (ModelState.IsValid)
            {
                try
                {
                    var licenceResult = await _ContactService.CreateAsync(contactAddModel);
                    if (licenceResult != null)
                        return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.Created };
                    response.Errors.Add("Unable to insert Contact. Please try again later.");
                    return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
                }
                catch (Exception ex)
                {
                    response.Errors.Add(ex.Message); // Add the error message from the CreateAsync method
                    return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
                }
            }
            response.Errors.Add("Invalid model state.");
            return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status304NotModified, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ReturnResult))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = new ReturnResult();
            if (id != Guid.Empty)
            {
                var success = await _ContactService.DeleteAsync(id);
                if (success)
                    return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
                response.Errors.Add("Unable to delete licence. Please try again later.");
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.NotModified };
            }
            response.Errors.Add("Invalid ID provided.");
            return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status304NotModified, Type = typeof(ReturnResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ReturnResult))]
        public async Task<IActionResult> Update(ContactUpdateModal contactUpdateModal)
        {
            var response = new ReturnResult();
            if (ModelState.IsValid)
            {
                contactUpdateModal.IsDeleted = false;
                var result = await _ContactService.UpdateAsync(contactUpdateModal);
                if (result != null)
                    return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
                response.Errors.Add("Unable to update contact. Please try again later.");
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
            response.Errors.Add("Invalid model state.");
            return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
        #endregion
    }
}
