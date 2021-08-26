using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PageViewProfile : Profile
    {
        public PageViewProfile()
        {
            CreateMap<Domain.Page, PageViewModel>();
        }
    }
}
