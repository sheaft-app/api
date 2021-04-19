using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateAllCatalogPricesInputType : SheaftInputType<UpdateAllCatalogPricesCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateAllCatalogPricesCommand> descriptor)
        {
            descriptor.Name("UpdateAllCatalogPricesInput");
            
            descriptor.Field(c => c.Percent);
            descriptor.Field(c => c.CatalogId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}