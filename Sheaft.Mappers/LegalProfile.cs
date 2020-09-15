using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;

namespace Sheaft.Mappers
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
