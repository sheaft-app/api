using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateCatalogInputType : SheaftInputType<CreateCatalogDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateCatalogDto> descriptor)
        {
            descriptor.Name("CreateCatalogInput");
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.IsAvailable);
            descriptor.Field(c => c.IsDefault);
        }
    }
}