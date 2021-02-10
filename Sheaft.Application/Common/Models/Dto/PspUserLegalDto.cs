using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Dto
{
    public class PspUserLegalDto : PspUserDto
    {
        public string ProofOfRegistration { get; set; }
        public string Statute { get; set; }
        public CountryIsoCode LegalRepresentativeCountryOfResidence { get; set; }
        public CountryIsoCode LegalRepresentativeNationality { get; set; }
        public DateTime? LegalRepresentativeBirthday { get; set; }
        public string LegalRepresentativeEmail { get; set; }
        public string LegalRepresentativeAddressObsolete { get; set; }
        public AddressDto LegalRepresentativeAddress { get; set; }
        public string LegalRepresentativeLastName { get; set; }
        public string LegalRepresentativeFirstName { get; set; }
        public string HeadquartersAddressObsolete { get; set; }
        public AddressDto HeadquartersAddress { get; set; }
        public LegalKind LegalPersonType { get; set; }
        public string CompanyNumber { get; set; }
        public string Name { get; set; }
        public string ShareholderDeclaration { get; set; }
        public string LegalRepresentativeProofOfIdentity { get; set; }
    }
}