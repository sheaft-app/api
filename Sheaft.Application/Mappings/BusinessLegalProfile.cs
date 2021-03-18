using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class BusinessLegalProfile : Profile
    {
        public BusinessLegalProfile()
        {
            CreateMap<BusinessLegal, BusinessLegalDto>();
        }
    }
}
