using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CatalogPriceInputType : SheaftInputType<CatalogProductDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CatalogProductDto> descriptor)
        {
            descriptor.Name("CatalogPriceInput");
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.WholeSalePricePerUnit);
        }
    }
}