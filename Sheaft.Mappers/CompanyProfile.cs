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

            CreateMap<Company, UserProfileDto>();

            CreateMap<Company, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                  .ForMember(d => d.BillingAddress, opt => opt.MapFrom(r => r.BillingAddress));

            CreateMap<Company, CompanyProfileDto>()
                .IncludeBase<Company, UserDto>();

            CreateMap<Producer, UserProfileDto>();

            CreateMap<Producer, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                  .ForMember(d => d.BillingAddress, opt => opt.MapFrom(r => r.BillingAddress));

            CreateMap<Producer, ProducerDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.BillingAddress, opt => opt.MapFrom(r => r.BillingAddress))
                .ForMember(d => d.Owner, opt => opt.MapFrom(r => 
                new OwnerDto {
                    CountryOfResidence = r.CountryOfResidence,
                    Nationality = r.Nationality,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Legal = r.Legal,
                    Birthdate = r.Birthdate,
                    Address = r.LegalAddress != null ? new AddressDto
                    {
                        Line1 = r.LegalAddress.Line1,
                        Line2 = r.LegalAddress.Line2,
                        Zipcode = r.LegalAddress.Zipcode,
                        City = r.LegalAddress.City,
                        Country = r.LegalAddress.Country
                    } : null
                }))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)));

            CreateMap<Store, UserProfileDto>();

            CreateMap<Store, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.BillingAddress, opt => opt.MapFrom(r => r.BillingAddress));

            CreateMap<Store, StoreDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.BillingAddress, opt => opt.MapFrom(r => r.BillingAddress))
                .ForMember(d => d.Owner, opt => opt.MapFrom(r =>
                new OwnerDto
                {
                    CountryOfResidence = r.CountryOfResidence,
                    Nationality = r.Nationality,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Legal = r.Legal,
                    Birthdate = r.Birthdate,
                    Address = r.LegalAddress != null ? new AddressDto
                    {
                        Line1 = r.LegalAddress.Line1,
                        Line2 = r.LegalAddress.Line2,
                        Zipcode = r.LegalAddress.Zipcode,
                        City = r.LegalAddress.City,
                        Country = r.LegalAddress.Country
                    } : null
                }))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours));

            CreateMap<RegisterStoreInput, RegisterStoreCommand>();
            CreateMap<RegisterProducerInput, RegisterProducerCommand>();
            CreateMap<SetCompanyLegalsInput, SetCompanyLegalsCommand>();
            CreateMap<UpdateStoreInput, UpdateStoreCommand>();
            CreateMap<UpdateProducerInput, UpdateProducerCommand>();
            CreateMap<UpdatePictureInput, UpdateUserPictureCommand>();
            CreateMap<IdWithReasonInput, DeleteCompanyCommand>();
        }
    }
}
