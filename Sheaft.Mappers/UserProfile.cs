using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Models.ViewModels;
using System;

namespace Sheaft.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(c => c.DepartmentId, opt => opt.MapFrom(d => d.Address != null ? d.Address.Department.Id : (Guid?)null));

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
            CreateMap<IdWithReasonInput, DeleteUserCommand>();
        }
    }
}
