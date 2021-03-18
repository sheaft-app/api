using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.Store.Commands;

namespace Sheaft.Services.Mappings
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
