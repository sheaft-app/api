using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class PurchaseOrderDeliveryProfile : Profile
    {
        public PurchaseOrderDeliveryProfile()
        {
            CreateMap<PurchaseOrderDelivery, ExpectedPurchaseOrderDeliveryDto>()
                 .ForMember(d => d.Day, opt => opt.MapFrom(r => r.ExpectedDeliveryDate.DayOfWeek));
        }
    }
}
