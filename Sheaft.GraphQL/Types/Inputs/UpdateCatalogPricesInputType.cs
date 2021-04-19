using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateCatalogPricesInputType : SheaftInputType<UpdateCatalogPricesCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateCatalogPricesCommand> descriptor)
        {
            descriptor.Name("UpdateCatalogPricesInput");
            
            descriptor.Field(c => c.Prices)
                .Type<NonNullType<ListType<ProductPriceInputType>>>();
            
            descriptor.Field(c => c.CatalogId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}