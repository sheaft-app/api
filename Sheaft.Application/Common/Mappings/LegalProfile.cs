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
            CreateMap<Legal, LegalViewModel>();
        }
    }
}
