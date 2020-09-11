using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
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
