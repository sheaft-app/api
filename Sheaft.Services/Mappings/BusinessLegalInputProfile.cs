using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.Legal.Commands;

namespace Sheaft.Services.Mappings
{
    public class BusinessLegalInputProfile : Profile
    {
        public BusinessLegalInputProfile()
        {
            CreateMap<CreateBusinessLegalDto, CreateBusinessLegalCommand>();
            CreateMap<UpdateBusinessLegalDto, UpdateBusinessLegalCommand>()
                .ForMember(c => c.LegalId, opt => opt.MapFrom(r => r.Id));;
        }
    }
}
