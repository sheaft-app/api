using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Cards.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class CardRegistrationProfile : Profile
    {
        public CardRegistrationProfile()
        {
            CreateMap<CreateCardRegistrationDto, CreateCardRegistrationCommand>();
        }
    }
}