using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class LegalProfile : Profile
    {
        public LegalProfile()
        {
            CreateMap<Legal, BaseLegalDto>();

            CreateMap<BusinessLegal, BusinessLegalDto>()
                .IncludeBase<Legal, BaseLegalDto>();

            CreateMap<ConsumerLegal, ConsumerLegalDto>()
                .IncludeBase<Legal, BaseLegalDto>();

            CreateMap<CreateBusinessLegalInput, CreateBusinessLegalCommand>();
            CreateMap<CreateConsumerLegalInput, CreateConsumerLegalCommand>();
            CreateMap<UpdateBusinessLegalInput, UpdateBusinessLegalCommand>();
            CreateMap<UpdateConsumerLegalInput, UpdateConsumerLegalCommand>();

            CreateMap<CreateUboInput, CreateUboCommand>();
            CreateMap<UpdateUboInput, UpdateUboCommand>();
            CreateMap<IdInput, RemoveUboCommand>();
        }
    }
}
