using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Models.ViewModels;
using System.Linq;

namespace Sheaft.Mappers
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<Business, BusinessViewModel>()
                  .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Business, UserProfileDto>();

            CreateMap<Business, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Business, BusinessProfileDto>()
                .IncludeBase<Business, UserDto>();

            CreateMap<Producer, UserProfileDto>();

            CreateMap<Producer, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Producer, ProducerDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))                
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)));

            CreateMap<Store, UserProfileDto>();

            CreateMap<Store, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Store, StoreDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours));

            CreateMap<RegisterStoreInput, RegisterStoreCommand>();
            CreateMap<RegisterProducerInput, RegisterProducerCommand>();
            CreateMap<SetBusinessLegalsInput, SetBusinessLegalsCommand>();
            CreateMap<UpdateStoreInput, UpdateStoreCommand>();
            CreateMap<UpdateProducerInput, UpdateProducerCommand>();
            CreateMap<UpdatePictureInput, UpdateUserPictureCommand>();
        }
    }
}
