using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RemoveProductsFromCatalogInputType : SheaftInputType<RemoveProductsFromCatalogCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RemoveProductsFromCatalogCommand> descriptor)
        {
            descriptor.Name("RemoveProductsFromCatalogInput");
            descriptor.Field(c => c.ProductIds)
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.CatalogId)
                .Name("id")
                .Type<NonNullType<IdType>>();
        }
    }
}