using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;
using System;

namespace Sheaft.Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<User, UserProfileDto>();

            CreateMap<User, UserDto>()
                .IncludeBase<User, UserProfileDto>()
                .ForMember(c => c.Address, opt => opt.MapFrom(d => d.Address));

            CreateMap<PurchaseOrderSender, UserProfileDto>();
            CreateMap<PurchaseOrderVendor, UserProfileDto>();

            CreateMap<PurchaseOrderSender, OrderUserViewModel>();
            CreateMap<PurchaseOrderVendor, OrderUserViewModel>();

            CreateMap<IdInput, GenerateUserCodeCommand>();
            CreateMap<UpdatePictureInput, UpdateUserPictureCommand>();
            CreateMap<IdWithReasonInput, RemoveUserCommand>();
        }
    }
}
