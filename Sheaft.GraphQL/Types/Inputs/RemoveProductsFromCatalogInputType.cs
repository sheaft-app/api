using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RemoveProductsFromCatalogInputType : SheaftInputType<RemoveProductsFromCatalogCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RemoveProductsFromCatalogCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RemoveProductsFromCatalogInput");
            
            descriptor
                .Field(c => c.ProductIds)
                .Name("productIds")
                .ID(nameof(Product));

            descriptor
                .Field(c => c.CatalogId)
                .Name("id")
                .ID(nameof(Catalog));
        }
    }
}