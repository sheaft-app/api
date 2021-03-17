using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Domain;

namespace Sheaft.Mappings
{
    public class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
            CreateMap<BankAccount, BankAccountDto>();
            
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
