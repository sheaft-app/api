using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PageViewProfile : Profile
    {
        public PageViewProfile()
        {
            CreateMap<Domain.Page, PageViewModel>();
            CreateMap<PageDto, PageViewModel>();
            CreateMap<PageViewModel, PageDto>();
        }
    }
}
