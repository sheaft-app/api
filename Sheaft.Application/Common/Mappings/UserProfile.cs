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

            CreateMap<Domain.User, UserProfileDto>();

            CreateMap<Domain.User, UserDto>()
                .IncludeBase<Domain.User, UserProfileDto>()
                .ForMember(c => c.Address, opt => opt.MapFrom(d => d.Address));

            CreateMap<PurchaseOrderSender, UserProfileDto>();
            CreateMap<PurchaseOrderVendor, UserProfileDto>();

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
