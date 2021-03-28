using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CatalogPriceInputType : SheaftInputType<CatalogPriceDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CatalogPriceDto> descriptor)
        {
            descriptor.Name("CatalogPriceInput");
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.WholeSalePricePerUnit);
        }
    }
}