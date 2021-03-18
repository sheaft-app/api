using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class QuickOrderProfile : Profile
    {
        public QuickOrderProfile()
        {
            CreateMap<Domain.QuickOrder, QuickOrderDto>();
        }
    }
}
