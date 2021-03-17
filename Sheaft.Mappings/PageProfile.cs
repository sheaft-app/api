using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Mappings
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<Domain.Page, PageDto>();
            CreateMap<Domain.Page, PageViewModel>();
        }
    }
}
