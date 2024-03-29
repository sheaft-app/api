﻿using System.Linq;
using AutoMapper;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ProducerViewProfile : Profile
    {
        public ProducerViewProfile()
        {
            CreateMap<Producer, UserViewModel>();
            CreateMap<Domain.Producer, ProducerViewModel>()
                .IncludeBase<Producer, UserViewModel>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)));
        }
    }
}