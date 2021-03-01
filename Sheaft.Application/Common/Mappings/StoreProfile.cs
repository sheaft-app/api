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

            CreateMap<Domain.Store, UserDto>()
                .ForMember(c => c.Summary, opt => opt.MapFrom(e => e.ProfileInformation.Summary))
                .ForMember(c => c.Description, opt => opt.MapFrom(e => e.ProfileInformation.Description))
                .ForMember(c => c.Facebook, opt => opt.MapFrom(e => e.ProfileInformation.Facebook))
                .ForMember(c => c.Instagram, opt => opt.MapFrom(e => e.ProfileInformation.Instagram))
                .ForMember(c => c.Twitter, opt => opt.MapFrom(e => e.ProfileInformation.Twitter))
                .ForMember(c => c.Website, opt => opt.MapFrom(e => e.ProfileInformation.Website))
                .ForMember(c => c.Pictures, opt => opt.MapFrom(e => e.ProfileInformation.Pictures));

            CreateMap<Domain.Store, StoreDto>()
                .IncludeBase<Domain.Store, UserDto>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)));;
            
            CreateMap<RegisterStoreInput, RegisterStoreCommand>();
            CreateMap<UpdateStoreInput, UpdateStoreCommand>()
                .ForMember(c => c.StoreId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
