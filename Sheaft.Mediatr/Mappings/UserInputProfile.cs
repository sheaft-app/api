﻿using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class UserInputProfile : Profile
    {
        public UserInputProfile()
        {
            CreateMap<UpdateResourceIdPictureDto, UpdateUserPictureCommand>()
                .ForMember(c => c.UserId, opt => opt.MapFrom(d => d.Id));

            CreateMap<ResourceIdDto, GenerateUserCodeCommand>()
                    .ForMember(c => c.UserId, opt => opt.MapFrom(r => r.Id));
            CreateMap<ResourceIdWithReasonDto, RemoveUserCommand>()
                .ForMember(c => c.UserId, opt => opt.MapFrom(r => r.Id));;
        }
    }
}