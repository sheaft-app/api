using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class SirenLegalsType : ObjectType<SirenLegalsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SirenLegalsDto> descriptor)
        {
            descriptor.Field(c => c.NomUniteLegale)
                .Name("lastName");

            descriptor.Field(c => c.PrenomUsuelUniteLegale)
                .Name("firstName");
        }
    }
}
