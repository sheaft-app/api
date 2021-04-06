using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class AgreementProfile : Profile
    {
        public AgreementProfile()
        {
            CreateMap<Domain.Agreement, AgreementDto>();
        }
    }
}
