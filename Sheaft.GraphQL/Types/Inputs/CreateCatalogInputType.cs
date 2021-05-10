using HotChocolate.Types;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateCatalogInputType : SheaftInputType<CreateCatalogCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateCatalogCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateCatalogInput");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");

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