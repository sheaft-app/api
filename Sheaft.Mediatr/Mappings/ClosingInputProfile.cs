using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.BusinessClosing.Commands;
using Sheaft.Mediatr.DeliveryClosing.Commands;
using Sheaft.Mediatr.ProductClosing.Commands;

namespace Sheaft.Mediatr.Mappings
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
