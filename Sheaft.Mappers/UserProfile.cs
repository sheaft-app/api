﻿using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Interop.Enums;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(c => c.Address, opt => opt.MapFrom(d => d.Address))
                .ForMember(c => c.BillingAddress, opt => opt.MapFrom(d => d.BillingAddress));

            CreateMap<User, UserProfileDto>();

            CreateMap<User, UserDto>()
                .IncludeBase<User, UserProfileDto>()
                .ForMember(c => c.Address, opt => opt.MapFrom(d => d.Address))
                .ForMember(c => c.BillingAddress, opt => opt.MapFrom(d => d.BillingAddress));

            CreateMap<PurchaseOrderSender, UserProfileDto>();
            CreateMap<PurchaseOrderVendor, UserProfileDto>();

            CreateMap<PurchaseOrderSender, OrderUserViewModel>();
            CreateMap<PurchaseOrderVendor, OrderUserViewModel>();

            CreateMap<IdInput, GenerateUserCodeCommand>();
            CreateMap<RegisterConsumerInput, CreateConsumerCommand>();
            CreateMap<UpdateConsumerInput, UpdateConsumerCommand>();
            CreateMap<UpdatePictureInput, UpdateUserPictureCommand>();
            CreateMap<IdWithReasonInput, DeleteConsumerCommand>();
        }
    }
}
