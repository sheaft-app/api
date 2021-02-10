using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain.Enums;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types
{
    public class SirenBusinessType : ObjectType<SirenBusinessDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SirenBusinessDto> descriptor)
        {
            descriptor.Field(c => c.Siren);
            descriptor.Field(c => c.Nic);
            descriptor.Field(c => c.Siret);

            descriptor.Field("active")
                .Resolver(c => c.Parent<SirenBusinessDto>().UniteLegale?.EtatAdministratifUniteLegale == "A")
                .Type<NonNullType<BooleanType>>();

            descriptor.Field("kind")
                .Resolver(c =>
                {
                    switch (c.Parent<SirenBusinessDto>().UniteLegale?.CategorieJuridiqueUniteLegale)
                    {
                        case "":
                            return null;
                        case "1000":
                            return LegalKind.Individual;
                        default:
                            return LegalKind.Business;
                    }
                })
                .Type<LegalKindEnumType>();

            descriptor.Field("name")
                .Resolver(c => c.Parent<SirenBusinessDto>().UniteLegale?.DenominationUsuelle1UniteLegale)
                .Type<StringType>();

            descriptor.Field(c => c.AdresseEtablissement)
                .Name("address")
                .Type<SirenAddressType>();

            descriptor.Field(c => c.UniteLegale)
                .Name("owner")
                .Type<SirenLegalsType>();
        }
    }
}
