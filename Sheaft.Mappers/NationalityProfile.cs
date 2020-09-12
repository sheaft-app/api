﻿using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
{
    public class NationalityProfile : Profile
    {
        public NationalityProfile()
        {
            CreateMap<Nationality, NationalityDto>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}