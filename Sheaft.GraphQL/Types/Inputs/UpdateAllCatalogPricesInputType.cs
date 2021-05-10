using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateAllCatalogPricesInputType : SheaftInputType<UpdateAllCatalogPricesCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateAllCatalogPricesCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateAllCatalogPricesInput");

            descriptor
                .Field(c => c.Percent)
                .Name("percent");
            
            descriptor
                .Field(c => c.CatalogId)
                .Name("id")
                .ID(nameof(Catalog));
        }
    }
}