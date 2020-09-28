using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;
using System;

namespace Sheaft.Application.Mappers
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>();
            CreateMap<Tag, TagViewModel>();
        }
    }
}
