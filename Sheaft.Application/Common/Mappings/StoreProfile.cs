using System.Linq;
using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Store.Commands;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Mappings
{
    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            CreateMap<Business, StoreViewModel>()
                  .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Domain.Store, StoreViewModel>()
                .IncludeBase<Business, StoreViewModel>()
                  .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours))
                  .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)));

            CreateMap<Domain.Store, UserProfileDto>();

            CreateMap<Domain.Store, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Domain.Store, StoreDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours));

            CreateMap<RegisterStoreInput, RegisterStoreCommand>();
            CreateMap<UpdateStoreInput, UpdateStoreCommand>()
                .ForMember(c => c.StoreId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
