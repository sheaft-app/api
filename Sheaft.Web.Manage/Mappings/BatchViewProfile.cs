using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class BatchViewProfile : Profile
    {
        public BatchViewProfile()
        {
            CreateMap<Domain.Batch, BatchViewModel>();
            CreateMap<Domain.Batch, ShortBatchViewModel>();
        }
    }
}