using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateCatalogPricesInputType : SheaftInputType<UpdateCatalogPricesCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateCatalogPricesCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateCatalogPricesInput");
            
            descriptor
                .Field(c => c.Prices)
                .Name("prices")
                .Type<NonNullType<ListType<ProductPriceInputType>>>();
            
            descriptor
                .Field(c => c.CatalogId)
                .Name("id")
                .ID(nameof(Catalog));
        }
    }
}