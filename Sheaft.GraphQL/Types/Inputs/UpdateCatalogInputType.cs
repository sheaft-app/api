using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateCatalogInputType : SheaftInputType<UpdateCatalogCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateCatalogCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateCatalogInput");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");

            descriptor
                .Field(c => c.CatalogId)
                .Name("id")
                .ID(nameof(Catalog));

            descriptor
                .Field(c => c.IsAvailable)
                .Name("available");
            
            descriptor
                .Field(c => c.IsDefault)
                .Name("isDefault");
            
            descriptor
                .Field(c => c.Products)
                .Name("products")
                .Type<ListType<ProductPriceInputType>>();
        }
    }
}