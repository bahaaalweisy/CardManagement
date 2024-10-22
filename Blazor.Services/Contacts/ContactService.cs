using AutoMapper;
using CardManagement.Core.Models.Common;
using CardManagement.Core.Models.Pagination;
using CardManagement.Core;
using CardManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CardManagement.Core.Domain.Activities;
using CardManagement.Core.Models.Contact;
namespace CardManagement.Services.Contacts
{
    public class ContactService : IContactService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Contact> _ContactsRepository;

        public ContactService(IMapper mapper, IRepository<Contact> contactsRepository)
        {
            _mapper = mapper;
            _ContactsRepository = contactsRepository;
        }

        public async Task<PagedList<GetAllContactModel>> GetPaginatedListAsync(PagedRequestListModel pagedRequest)
        {
            if (pagedRequest == null)
                throw new ArgumentNullException(nameof(pagedRequest));
            var licenceQuery = _ContactsRepository.FindAllAsQueryable(x => !x.IsDeleted);
            if (!string.IsNullOrEmpty(pagedRequest.SearchText))
            {
                licenceQuery = licenceQuery.Where(a => a.Email.Contains(pagedRequest.SearchText) || a.PhoneNumber.Contains(pagedRequest.SearchText));
            }
            var licences = await licenceQuery.ToListAsync();
            if(licences.Count() == 0)
                throw new Exception("Data Not Found");

            var list = _mapper.Map<List<GetAllContactModel>>(licences).AsQueryable();
            return list.ToPaginatedList(1, list.Count());
        }

        public async Task<ContactAddModel> CreateAsync(ContactAddModel contactAddModel)
        {
            if (contactAddModel == null)
                throw new ArgumentNullException(nameof(contactAddModel));

            var existingcontact = _ContactsRepository
                .FindAllAsQueryable(x =>
                    (!x.IsDeleted && x.Email == contactAddModel.Email) ||
                    (!x.IsDeleted && x.PhoneNumber == contactAddModel.PhoneNumber)
                );
            var existingLicenceWithSameName = await existingcontact.FirstOrDefaultAsync();

            if (existingLicenceWithSameName != null)
            {
                if (existingLicenceWithSameName.Email == contactAddModel.Email && existingLicenceWithSameName.PhoneNumber == contactAddModel.PhoneNumber)
                {
                    throw new Exception("Email and Phone Number already exists");
                }
                if (existingLicenceWithSameName.Email == contactAddModel.Email)
                {
                    throw new Exception("Email already exists");
                }
                if (existingLicenceWithSameName.PhoneNumber == contactAddModel.PhoneNumber)
                {
                    throw new Exception("Phone Number already exists");
                }
            }

            var contactAdd = _mapper.Map<Contact>(contactAddModel);
            contactAdd.CreatedOnUtc = DateTime.UtcNow;
            contactAdd.UpdatedOnUtc = DateTime.UtcNow;

            await _ContactsRepository.InsertAsync(contactAdd);

            var createdLicenceModel = _mapper.Map<ContactAddModel>(contactAdd);

            return createdLicenceModel;
        }


        public async Task<GetAllContactModel> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            var ContactQuery = _ContactsRepository.FindAllAsQueryable(x => x.Id == id && !x.IsDeleted);
            var Contact = await ContactQuery.FirstOrDefaultAsync();
            if (Contact == null)
                return null;
            return _mapper.Map<GetAllContactModel>(Contact);
        }


        public async Task<ContactUpdateModal> UpdateAsync(ContactUpdateModal contactUpdateModal)
        {
            if (contactUpdateModal == null)
                throw new ArgumentNullException(nameof(contactUpdateModal));

            if (contactUpdateModal.Id == Guid.Empty)
                throw new ArgumentException("Invalid ID.", nameof(contactUpdateModal.Id));

            var queryableLicences = await _ContactsRepository.FindAllAsync(x => x.Id == contactUpdateModal.Id && !x.IsDeleted);
            var Contacts = await queryableLicences.FirstOrDefaultAsync();

            if (Contacts == null)
                return null;

            var existingContact = _ContactsRepository.FindAllAsQueryable(x => x.Email == contactUpdateModal.Email && x.Id != contactUpdateModal.Id && !x.IsDeleted || x.PhoneNumber == contactUpdateModal.PhoneNumber && x.Id != contactUpdateModal.Id && !x.IsDeleted);
            var existingContactWithSameName = await existingContact.FirstOrDefaultAsync();

            if (existingContactWithSameName != null)
            {
                if (existingContactWithSameName.Email == contactUpdateModal.Email && existingContactWithSameName.PhoneNumber == contactUpdateModal.PhoneNumber)
                {
                    throw new Exception("Email and Phone Number already exists");
                }
                if (existingContactWithSameName.Email == contactUpdateModal.Email)
                {
                    throw new Exception("Email already exists");
                }
                if (existingContactWithSameName.PhoneNumber == contactUpdateModal.PhoneNumber)
                {
                    throw new Exception("Phone Number already exists");
                }
            }

            _mapper.Map(contactUpdateModal, Contacts);
            Contacts.UpdatedOnUtc = DateTime.UtcNow;

            var result = await _ContactsRepository.UpdateAsync(Contacts);

            if (result.Succeeded)
            {
                return _mapper.Map<ContactUpdateModal>(Contacts);
            }
            else
            {
                throw new InvalidOperationException("Update operation failed.");
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var queryableLicences = await _ContactsRepository.FindAllAsync(x => x.Id == id && !x.IsDeleted);

            var licence = await queryableLicences.FirstOrDefaultAsync();

            if (licence != null)
            {
                licence.IsDeleted = true;
                licence.UpdatedOnUtc = DateTime.UtcNow; 
                await _ContactsRepository.UpdateAsync(licence);
                return true;
            }

            return false;
        }
    }
}
