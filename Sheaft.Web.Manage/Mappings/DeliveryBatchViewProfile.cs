using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DeliveryBatchViewProfile : Profile
    {
        public DeliveryBatchViewProfile()
        {
            CreateMap<Domain.DeliveryBatch, DeliveryBatchViewModel>();
            CreateMap<Domain.DeliveryBatch, ShortDeliveryBatchViewModel>();
        }
    }
}