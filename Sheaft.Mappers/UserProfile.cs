using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;

namespace Sheaft.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserProfileDto>()
                .ForMember(c => c.Name, opt => opt.MapFrom(d => $"{d.FirstName} {d.LastName}"))
                .ForMember(c => c.ShortName, opt => opt.MapFrom(d => $"{d.FirstName} {d.LastName.Substring(0, 1)}"))
                .ForMember(c => c.Initials, opt => opt.MapFrom(d => $"{d.FirstName.Substring(0, 1)} {d.LastName.Substring(0, 1)}"));

            CreateMap<User, UserDto>()
                .IncludeBase<User, UserProfileDto>()
                .ForMember(c => c.Department, opt => opt.MapFrom(d => d.Department));

            CreateMap<PurchaseOrderUser, UserProfileDto>();
            CreateMap<PurchaseOrderSender, UserProfileDto>()
                .IncludeBase<PurchaseOrderUser, UserProfileDto>();
            CreateMap<PurchaseOrderVendor, UserProfileDto>()
                .IncludeBase<PurchaseOrderUser, UserProfileDto>();

            CreateMap<IdInput, GenerateUserSponsoringCodeCommand>();
            CreateMap<RegisterConsumerInput, RegisterConsumerCommand>();
            CreateMap<RegisterOwnerInput, RegisterOwnerCommand>();
            CreateMap<UpdateUserInput, UpdateUserCommand>();
            CreateMap<UpdatePictureInput, UpdateUserPictureCommand>();
            CreateMap<IdWithReasonInput, DeleteUserCommand>();
        }
    }
}
