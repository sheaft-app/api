using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class BankAccountViewProfile : Profile
    {
        public BankAccountViewProfile()
        {
            CreateMap<BankAccount, BankAccountDto>();
            
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
            
            CreateMap<BankAccountShortViewModel, BankAccountDto>();
            CreateMap<BankAccountViewModel, BankAccountDto>()
                .ForMember(c => c.Line1, opt => opt.MapFrom(r => r.Address.Line1))
                .ForMember(c => c.Line2, opt => opt.MapFrom(r => r.Address.Line2))
                .ForMember(c => c.Zipcode, opt => opt.MapFrom(r => r.Address.Zipcode))
                .ForMember(c => c.City, opt => opt.MapFrom(r => r.Address.City));

            CreateMap<BankAccountDto, BankAccountShortViewModel>();
            CreateMap<BankAccountDto, BankAccountViewModel>()
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