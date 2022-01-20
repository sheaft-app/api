using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class CompanyLegals
    {
        protected CompanyLegals()
        {
        }

        internal CompanyLegals(Guid companyId, LegalKind kind, string name, string identifier, string vatNumber, bool isExemptedFromVat, LegalsAddress address)
        {
            CompanyId = companyId;
            Kind = kind;
            Name = name;
            Identifier = identifier;
            VATNumber = vatNumber;
            IsExemptedFromVAT = isExemptedFromVat;
            Address = address;
        }

        public Guid CompanyId { get; set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public LegalKind Kind { get; protected set; }
        public string Name { get; private set; }
        public string Identifier { get; private set; }
        public string VATNumber { get; private set; }
        public bool IsExemptedFromVAT { get; private set; }
        public LegalsAddress Address { get; private set; }

        public void SetRegistrationKind(RegistrationKind kind, string city, string code)
        {
            if(kind == RegistrationKind.RCS)
                Identifier = $"{kind:G} {city.Trim()} {Identifier.Substring(0, 9)} ";
            
            if(kind == RegistrationKind.RM)
                Identifier = $"{kind:G} {Identifier.Substring(0, 9)} {code.Trim()}";
        }
    }
}