using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddOrUpdateProductsToCatalogInputType : SheaftInputType<AddOrUpdateProductsToCatalogCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddOrUpdateProductsToCatalogCommand> descriptor)
        {
            descriptor.Name("AddOrUpdateProductsToCatalogInput");
            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ProductPriceInputType>>>();

            descriptor.Field(c => c.CatalogId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}