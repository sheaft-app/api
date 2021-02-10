using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.QuickOrder.Commands;

namespace Sheaft.Application.Common.Mappings
{
    public class QuickOrderProfile : Profile
    {
        public QuickOrderProfile()
        {
            CreateMap<Domain.QuickOrder, QuickOrderDto>()
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
