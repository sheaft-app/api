using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Picture.Commands;
using Sheaft.Application.User.Commands;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Domain.User, UserViewModel>();
            CreateMap<Domain.User, UserProfileViewModel>();

            CreateMap<Domain.User, UserDto>()
                .ForMember(c => c.Summary, opt => opt.MapFrom(e => e.ProfileInformation.Summary))
                .ForMember(c => c.Description, opt => opt.MapFrom(e => e.ProfileInformation.Description))
                .ForMember(c => c.Facebook, opt => opt.MapFrom(e => e.ProfileInformation.Facebook))
                .ForMember(c => c.Instagram, opt => opt.MapFrom(e => e.ProfileInformation.Instagram))
                .ForMember(c => c.Twitter, opt => opt.MapFrom(e => e.ProfileInformation.Twitter))
                .ForMember(c => c.Website, opt => opt.MapFrom(e => e.ProfileInformation.Website))
                .ForMember(c => c.Pictures, opt => opt.MapFrom(e => e.ProfileInformation.Pictures));

            CreateMap<PurchaseOrderSender, UserDto>();
            CreateMap<PurchaseOrderVendor, UserDto>();

            CreateMap<PurchaseOrderSender, UserProfileViewModel>();
            CreateMap<PurchaseOrderVendor, UserProfileViewModel>();

            CreateMap<UpdatePictureInput, UpdateUserPictureCommand>()
                .ForMember(c => c.UserId, opt => opt.MapFrom(d => d.Id));

            CreateMap<IdInput, GenerateUserCodeCommand>()
                    .ForMember(c => c.UserId, opt => opt.MapFrom(r => r.Id));
            CreateMap<IdWithReasonInput, RemoveUserCommand>()
                .ForMember(c => c.UserId, opt => opt.MapFrom(r => r.Id));;
        }
    }
}
