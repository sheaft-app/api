using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PickingProductViewProfile : Profile
    {
        public PickingProductViewProfile()
        {
            CreateMap<Domain.PickingProduct, PickingProductViewModel>();
        }
    }
}