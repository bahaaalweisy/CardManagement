using AutoMapper;
using CardManagement.Core.Domain.Activities;
using CardManagement.Core.Domain.Common;
using CardManagement.Core.Domain.Users;
using CardManagement.Core.Models.Account;
using CardManagement.Core.Models.Cards;
using CardManagement.Core.Models.Common;
using CardManagement.Core.Models.Contact;
using CardManagement.Core.Models.Users;
using System.ComponentModel;


namespace CardManagementApis.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            // User mappings
            CreateMap<SaveUserDetailModel, User>().ReverseMap();
            CreateMap<UserDetailModel, User>().ReverseMap();

            // Contact mappings
            CreateMap<ContactUpdateModal, Contact>().ReverseMap();
            CreateMap<ContactAddModel, Contact>().ReverseMap();
            CreateMap<GetAllContactModel, Contact>().ReverseMap();

            // Card mappings
            CreateMap<CardsUpdateModal, Card>().ReverseMap();
            CreateMap<CardsAddModel, Card>().ReverseMap();
            CreateMap<GetAllCardsModel, Card>().ReverseMap();
           
           
            CreateMap<CardApplicationModel, Card>().ReverseMap();
            CreateMap<CardApplicationModel, Card>()
             .ForMember(dest => dest.PhotoPath, opt => opt.Ignore())
             .ReverseMap();

        }
    }
}