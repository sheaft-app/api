using System.Linq;
using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PreparedProductViewProfile : Profile
    {
        public PreparedProductViewProfile()
        {
            CreateMap<Domain.PreparedProduct, PreparedProductViewModel>()
                .ForMember(c => c.Batches, opt => opt.MapFrom(e => e.Batches.Select(b => b.Batch)));
        }
    }
}