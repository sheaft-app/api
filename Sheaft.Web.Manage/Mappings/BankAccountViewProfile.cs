using AutoMapper;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class BankAccountViewProfile : Profile
    {
        public BankAccountViewProfile()
        {
            CreateMap<BankAccount, BankAccountShortViewModel>();
            CreateMap<BankAccount, BankAccountViewModel>()
                .ForMember(c => c.Address, opt => opt.MapFrom(r => new AddressViewModel
                {
                    Line1 = r.Line1,
                    Line2 = r.Line2,
                    Zipcode = r.Zipcode,
                    City = r.City,
                    Country = r.Country,
                }));
        }
    }
}