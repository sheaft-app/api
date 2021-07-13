using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SirenLegalsDtoType : SheaftOutputType<SirenLegalsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SirenLegalsDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SirenLegals");
            
            descriptor
                .Field(c => c.NomUniteLegale)
                .Name("lastName");

            descriptor
                .Field(c => c.PrenomUsuelUniteLegale)
                .Name("firstName");
            
            descriptor
                .Field(c => c.NumeroTvaIntra)
                .Name("vatNumber");
            
            descriptor
                .Field(c => c.DenominationUsuelle1UniteLegale)
                .Name("name");
        }
    }
}
