using AutoMapper;
using Sheaft.Application.BusinessClosing.Commands;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.DeliveryClosing.Commands;
using Sheaft.Application.ProductClosing.Commands;

namespace Sheaft.Application.Common.Mappings
{
    public class ClosingProfile : Profile
    {
        public ClosingProfile()
        {
            CreateMap<Domain.BusinessClosing, ClosingDto>();
            CreateMap<Domain.DeliveryClosing, ClosingDto>();
            CreateMap<Domain.ProductClosing, ClosingDto>();

            CreateMap<CreateBusinessClosingsInput, CreateBusinessClosingsCommand>();
            CreateMap<CreateDeliveryClosingsInput, CreateDeliveryClosingsCommand>();
            CreateMap<CreateProductClosingsInput, CreateProductClosingsCommand>();

            CreateMap<UpdateClosingInput, UpdateBusinessClosingCommand>()
                .ForMember(c => c.ClosingId, opt => opt.MapFrom(e => e.Id));
            CreateMap<UpdateClosingInput, UpdateDeliveryClosingCommand>()
                .ForMember(c => c.ClosingId, opt => opt.MapFrom(e => e.Id));
            CreateMap<UpdateClosingInput, UpdateProductClosingCommand>()
                .ForMember(c => c.ClosingId, opt => opt.MapFrom(e => e.Id));
            
            CreateMap<IdsInput, DeleteBusinessClosingsCommand>()
                .ForMember(e => e.ClosingIds, opt => opt.MapFrom(a => a.Ids));
            
            CreateMap<IdsInput, DeleteDeliveryClosingsCommand>()
                .ForMember(e => e.ClosingIds, opt => opt.MapFrom(a => a.Ids));

            CreateMap<IdsInput, DeleteProductClosingsCommand>()
                .ForMember(e => e.ClosingIds, opt => opt.MapFrom(a => a.Ids));
        }
    }
}
