using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddOrUpdateProductsToCatalogInputType : SheaftInputType<AddOrUpdateProductsToCatalogCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddOrUpdateProductsToCatalogCommand> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("AddOrUpdateProductsToCatalogInput");
            
            descriptor
                .Field(c => c.Products)
                .Name("products")
                .Type<NonNullType<ListType<ProductPriceInputType>>>();

            descriptor
                .Field(c => c.CatalogId)
                .Name("id")
                .ID(nameof(Catalog));
        }
    }
}