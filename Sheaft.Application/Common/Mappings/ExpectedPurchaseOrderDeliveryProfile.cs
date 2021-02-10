using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Mappings
{
    public class ExpectedPurchaseOrderDeliveryProfile : Profile
    {
        public ExpectedPurchaseOrderDeliveryProfile()
        {
            CreateMap<ExpectedPurchaseOrderDelivery, ExpectedPurchaseOrderDeliveryDto>()
                 .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                 .ForMember(d => d.Day, opt => opt.MapFrom(r => r.ExpectedDeliveryDate.DayOfWeek));

            CreateMap<ExpectedPurchaseOrderDelivery, ExpectedPurchaseOrderDeliveryViewModel>()
                 .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                 .ForMember(d => d.Day, opt => opt.MapFrom(r => r.ExpectedDeliveryDate.DayOfWeek));
        }
    }
}
