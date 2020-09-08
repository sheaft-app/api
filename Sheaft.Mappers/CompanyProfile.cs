using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Models.ViewModels;
using System.Linq;

namespace Sheaft.Mappers
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyViewModel>()
                  .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                  .ForMember(d => d.BillingAddress, opt => opt.MapFrom(r => r.BillingAddress));

            CreateMap<Company, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                  .ForMember(d => d.BillingAddress, opt => opt.MapFrom(r => r.BillingAddress));

            CreateMap<Company, CompanyProfileDto>()
                .IncludeBase<Company, UserDto>();

            CreateMap<Producer, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                  .ForMember(d => d.BillingAddress, opt => opt.MapFrom(r => r.BillingAddress));

            CreateMap<Producer, ProducerDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                  .ForMember(d => d.BillingAddress, opt => opt.MapFrom(r => r.BillingAddress))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)));

            CreateMap<Store, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                  .ForMember(d => d.BillingAddress, opt => opt.MapFrom(r => r.BillingAddress));

            CreateMap<Store, StoreDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                  .ForMember(d => d.BillingAddress, opt => opt.MapFrom(r => r.BillingAddress))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                 .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours));

            CreateMap<RegisterStoreInput, RegisterStoreCommand>();
            CreateMap<RegisterProducerInput, RegisterProducerCommand>();
            CreateMap<UpdateStoreInput, UpdateStoreCommand>();
            CreateMap<UpdateProducerInput, UpdateProducerCommand>();
            CreateMap<UpdatePictureInput, UpdateUserPictureCommand>();
            CreateMap<IdWithReasonInput, DeleteCompanyCommand>();
        }
    }
}
