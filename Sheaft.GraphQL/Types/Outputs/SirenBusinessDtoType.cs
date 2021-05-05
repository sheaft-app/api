using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SirenBusinessDtoType : SheaftOutputType<SirenBusinessDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SirenBusinessDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SirenBusiness");
            
            descriptor
                .Field(c => c.Siren)
                .Name("siren");
                
            descriptor
                .Field(c => c.Nic)
                .Name("nic");
                
            descriptor
                .Field(c => c.Siret)
                .Name("siret");

            descriptor
                .Field("active")
                .Resolver(c => c.Parent<SirenBusinessDto>().UniteLegale?.EtatAdministratifUniteLegale == "A")
                .Type<NonNullType<BooleanType>>();

            descriptor
                .Field("kind")
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
                });

            descriptor
                .Field("name")
                .Resolver(c => c.Parent<SirenBusinessDto>().UniteLegale?.DenominationUsuelle1UniteLegale)
                .Type<StringType>();

            descriptor
                .Field(c => c.AdresseEtablissement)
                .Name("address")
                .Type<SirenAddressDtoType>();

            descriptor
                .Field(c => c.UniteLegale)
                .Name("owner")
                .Type<SirenLegalsDtoType>();
        }
    }
}
