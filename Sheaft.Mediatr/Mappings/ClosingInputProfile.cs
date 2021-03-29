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
            CreateMap<UpdateOrCreateResourceIdClosingDto, UpdateOrCreateBusinessClosingCommand>()
                .ForMember(c => c.UserId, opt => opt.MapFrom(d => d.Id));
            CreateMap<UpdateOrCreateResourceIdClosingDto, UpdateOrCreateDeliveryClosingCommand>()
                .ForMember(c => c.DeliveryId, opt => opt.MapFrom(d => d.Id));
            CreateMap<UpdateOrCreateResourceIdClosingDto, UpdateOrCreateProductClosingCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(d => d.Id));

            CreateMap<UpdateOrCreateResourceIdClosingsDto, UpdateOrCreateBusinessClosingsCommand>()
                .ForMember(c => c.UserId, opt => opt.MapFrom(d => d.Id));
            CreateMap<UpdateOrCreateResourceIdClosingsDto, UpdateOrCreateDeliveryClosingsCommand>()
                .ForMember(c => c.DeliveryId, opt => opt.MapFrom(d => d.Id));
            CreateMap<UpdateOrCreateResourceIdClosingsDto, UpdateOrCreateProductClosingsCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(d => d.Id));
            
            CreateMap<ResourceIdsDto, DeleteBusinessClosingsCommand>()
                .ForMember(e => e.ClosingIds, opt => opt.MapFrom(a => a.Ids));
            
            CreateMap<ResourceIdsDto, DeleteDeliveryClosingsCommand>()
                .ForMember(e => e.ClosingIds, opt => opt.MapFrom(a => a.Ids));

            CreateMap<ResourceIdsDto, DeleteProductClosingsCommand>()
                .ForMember(e => e.ClosingIds, opt => opt.MapFrom(a => a.Ids));
        }
    }
}
