using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Application.Common.Mappings
{
    public class LegalProfile : Profile
    {
        public LegalProfile()
        {
            CreateMap<Domain.Legal, BaseLegalDto>();
            CreateMap<Domain.Legal, LegalViewModel>();
        }
    }
}
