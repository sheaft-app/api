using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CatalogType : ObjectType<CatalogDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CatalogDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.ProductsCount);
            descriptor.Field(c => c.IsAvailable);
            descriptor.Field(c => c.IsDefault);

            descriptor.Field(c => c.Products)
                .Type<ListType<CatalogProductType>>();
        }
    }
}