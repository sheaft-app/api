using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.BusinessClosing.Commands;
using Sheaft.Services.DeliveryClosing.Commands;
using Sheaft.Services.ProductClosing.Commands;

namespace Sheaft.Services.Mappings
{
    public class ClosingInputProfile : Profile
    {
        public ClosingInputProfile()
        {
            CreateMap<CreateResourceIdClosingsDto, CreateBusinessClosingsCommand>()
                .ForMember(c => c.UserId, opt => opt.MapFrom(d => d.Id));
            CreateMap<CreateResourceIdClosingsDto, CreateDeliveryClosingsCommand>()
                .ForMember(c => c.DeliveryId, opt => opt.MapFrom(d => d.Id));
            CreateMap<CreateResourceIdClosingsDto, CreateProductClosingsCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(d => d.Id));

            CreateMap<UpdateClosingDto, UpdateBusinessClosingCommand>()
                .ForMember(c => c.ClosingId, opt => opt.MapFrom(e => e.Id));
            CreateMap<UpdateClosingDto, UpdateDeliveryClosingCommand>()
                .ForMember(c => c.ClosingId, opt => opt.MapFrom(e => e.Id));
            CreateMap<UpdateClosingDto, UpdateProductClosingCommand>()
                .ForMember(c => c.ClosingId, opt => opt.MapFrom(e => e.Id));
            
            CreateMap<ResourceIdsDto, DeleteBusinessClosingsCommand>()
                .ForMember(e => e.ClosingIds, opt => opt.MapFrom(a => a.Ids));
            
            CreateMap<ResourceIdsDto, DeleteDeliveryClosingsCommand>()
                .ForMember(e => e.ClosingIds, opt => opt.MapFrom(a => a.Ids));

            CreateMap<ResourceIdsDto, DeleteProductClosingsCommand>()
                .ForMember(e => e.ClosingIds, opt => opt.MapFrom(a => a.Ids));
        }
    }
}
