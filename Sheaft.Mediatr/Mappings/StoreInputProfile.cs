using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Store.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class StoreInputProfile : Profile
    {
        public StoreInputProfile()
        {
            CreateMap<RegisterStoreDto, RegisterStoreCommand>();
            CreateMap<UpdateStoreDto, UpdateStoreCommand>()
                .ForMember(c => c.StoreId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
