using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Application.Models;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Mappers
{
    public class CardRegistrationProfile : Profile
    {
        public CardRegistrationProfile()
        {
            CreateMap<CardRegistration, CardRegistrationDto>();
            CreateMap<CreateCardRegistrationInput, CreateCardRegistrationCommand>();
            CreateMap<ValidateCardRegistrationInput, ValidateCardRegistrationCommand>();
        }
    }
}
