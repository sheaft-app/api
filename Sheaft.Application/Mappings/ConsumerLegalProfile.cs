using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class ConsumerLegalProfile : Profile
    {
        public ConsumerLegalProfile()
        {
            CreateMap<ConsumerLegal, ConsumerLegalDto>();
        }
    }
}
