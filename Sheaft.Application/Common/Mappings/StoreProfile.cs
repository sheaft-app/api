using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;
using System.Linq;

namespace Sheaft.Application.Mappers
{
    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            CreateMap<Business, StoreViewModel>()
                  .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Store, StoreViewModel>()
                .IncludeBase<Business, StoreViewModel>()
                  .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours))
                  .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)));

            CreateMap<Store, UserProfileDto>();

            CreateMap<Store, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Store, StoreDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours));

            CreateMap<RegisterStoreInput, RegisterStoreCommand>();
            CreateMap<UpdateStoreInput, UpdateStoreCommand>();
        }
    }
}
