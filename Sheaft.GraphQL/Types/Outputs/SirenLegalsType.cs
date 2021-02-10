using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
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
