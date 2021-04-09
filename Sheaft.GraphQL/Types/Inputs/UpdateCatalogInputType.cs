using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateCatalogInputType : SheaftInputType<UpdateCatalogDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateCatalogDto> descriptor)
        {
            descriptor.Name("UpdateCatalogInput");
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.IsAvailable);
            descriptor.Field(c => c.IsDefault);
        }
    }
}