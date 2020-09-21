using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class SirenBusinessType : ObjectType<SirenBusinessDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SirenBusinessDto> descriptor)
        {
            descriptor.Field(c => c.Siren);
            descriptor.Field(c => c.Nic);
            descriptor.Field(c => c.Siret);

            descriptor.Field("name")
                .Resolver(c => c.Parent<SirenBusinessDto>().UniteLegale.DenominationUsuelle1UniteLegale);

            descriptor.Field(c => c.AdresseEtablissement)
                .Name("address")
                .Type<SirenAddressType>();
            descriptor.Field(c => c.UniteLegale)

                .Name("owner")
                .Type<SirenLegalsType>();
        }
    }
}
