using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class CardRegistrationProfile : Profile
    {
        public CardRegistrationProfile()
        {
            CreateMap<CreateCardRegistrationInput, CreateCardRegistrationCommand>();
        }
    }
}
