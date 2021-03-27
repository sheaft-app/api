﻿using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class QuickOrderProductQuantityProfile : Profile
    {
        public QuickOrderProductQuantityProfile()
        {
            CreateMap<QuickOrderProduct, QuickOrderProductQuantityDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(r => r.Product.Id))
                .ForMember(d => d.Reference, opt => opt.MapFrom(r => r.Product.Reference))
                .ForMember(d => d.Name, opt => opt.MapFrom(r => r.Product.Name))
                .ForMember(d => d.Returnable, opt => opt.MapFrom(r => r.Product.Returnable));
        }
    }
}