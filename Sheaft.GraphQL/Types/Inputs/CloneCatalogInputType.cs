using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CloneCatalogInputType : SheaftInputType<CloneCatalogDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CloneCatalogDto> descriptor)
        {
            descriptor.Name("CloneCatalogInput");
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Percent);
        }
    }
}