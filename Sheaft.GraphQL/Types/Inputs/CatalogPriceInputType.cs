using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CatalogPriceInputType : SheaftInputType<CatalogPriceInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CatalogPriceInputDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CatalogPriceInput");
            
            descriptor
                .Field(c => c.CatalogId)
                .Name("id")
                .ID(nameof(Catalog));
                
            descriptor
                .Field(c => c.WholeSalePricePerUnit)
                .Name("wholeSalePricePerUnit");
        }
    }
}