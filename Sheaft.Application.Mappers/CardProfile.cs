using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Mappers
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<Card, CardDto>();
        }
    }
}
