using System.Linq;
using AutoMapper;
using Sheaft.Application.Extensions;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain.Enum;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ProductViewProfile : Profile
    {
        public ProductViewProfile()
        {
            CreateMap<Domain.Product, ProductViewModel>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)));
                
                
            CreateMap<ProductDto, ProductViewModel>();
            CreateMap<ProductViewModel, ProductDto>()
                .ForMember(d => d.IsReturnable, opt => opt.MapFrom(r => r.ReturnableId.HasValue));
        }
    }
}
