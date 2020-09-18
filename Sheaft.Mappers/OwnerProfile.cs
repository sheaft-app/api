﻿using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<Owner, OwnerDto>();
        }
    }
}