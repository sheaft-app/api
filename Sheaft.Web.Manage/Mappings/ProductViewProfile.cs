using System.Linq;
using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ProductViewProfile : Profile
    {
        public ProductViewProfile()
        {
            CreateMap<Domain.Product, ProductViewModel>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)));

            CreateMap<Domain.Product, ShortProductViewModel>();

        }
    }
}
