using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class QuickOrderProfile : Profile
    {
        public QuickOrderProfile()
        {
            CreateMap<QuickOrder, QuickOrderDto>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User))
                .ForMember(d => d.Products, opt => opt.MapFrom(r => r.Products));

            CreateMap<CreateQuickOrderInput, CreateQuickOrderCommand>();
            CreateMap<UpdateQuickOrderInput, UpdateQuickOrderCommand>();
            CreateMap<UpdateIdProductsQuantitiesInput, UpdateQuickOrderProductsCommand>();
            CreateMap<IdInput, SetDefaultQuickOrderCommand>();
            CreateMap<IdsWithReasonInput, DeleteQuickOrdersCommand>();
        }
    }
}
