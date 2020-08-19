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
                  .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)))
                  .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours))
                  .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Company, CompanyDto>()
                  .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                  .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours))
                  .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Company, ProducerDto>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Company, StoreDto>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Company, CompanyProfileDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Company, UserProfileDto>();

            CreateMap<RegisterCompanyInput, RegisterCompanyCommand>();
            CreateMap<UpdateCompanyInput, UpdateCompanyCommand>();
            CreateMap<UpdatePictureInput, UpdateCompanyPictureCommand>();
            CreateMap<IdWithReasonInput, DeleteCompanyCommand>();
        }
    }
}
