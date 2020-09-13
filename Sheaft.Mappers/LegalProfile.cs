using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class LegalProfile : Profile
    {
        public LegalProfile()
        {
            CreateMap<Legal, LegalProfileDto>();

            CreateMap<BusinessLegal, BusinessLegalDto>()
                .IncludeBase<Legal, LegalProfileDto>();

            CreateMap<ConsumerLegal, ConsumerLegalDto>()
                .IncludeBase<Legal, LegalProfileDto>();
        }
    }
}
