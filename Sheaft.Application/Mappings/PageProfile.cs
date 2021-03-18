using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<Domain.Page, PageDto>();
        }
    }
}
