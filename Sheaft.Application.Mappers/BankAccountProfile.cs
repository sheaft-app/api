using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
            CreateMap<BankAccount, BankAccountShortViewModel>();
            CreateMap<BankAccount, BankAccountViewModel>()
                .ForMember(c => c.Address, opt => opt.MapFrom(r => new AddressViewModel {
                    Line1 = r.Line1,
                    Line2 = r.Line2,
                    Zipcode = r.Zipcode,
                    City = r.City,
                    Country = r.Country,
                }));
        }
    }
}
