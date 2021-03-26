using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.QuickOrder.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class QuickOrderInputProfile : Profile
    {
        public QuickOrderInputProfile()
        {
            CreateMap<CreateQuickOrderDto, CreateQuickOrderCommand>();
            CreateMap<UpdateQuickOrderDto, UpdateQuickOrderCommand>()
                .ForMember(c => c.QuickOrderId, opt => opt.MapFrom(r => r.Id));;
            CreateMap<UpdateResourceIdProductsQuantitiesDto, UpdateQuickOrderProductsCommand>()
                .ForMember(c => c.QuickOrderId, opt => opt.MapFrom(r => r.Id));
            CreateMap<ResourceIdDto, SetDefaultQuickOrderCommand>()
                    .ForMember(c => c.QuickOrderId, opt => opt.MapFrom(r => r.Id));
            CreateMap<ResourceIdsWithReasonDto, DeleteQuickOrdersCommand>()
                .ForMember(c => c.QuickOrderIds, opt => opt.MapFrom(r => r.Ids));
        }
    }
}
