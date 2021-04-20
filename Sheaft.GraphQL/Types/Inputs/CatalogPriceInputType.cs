using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CatalogPriceInputType : SheaftInputType<CatalogPriceInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CatalogPriceInputDto> descriptor)
        {
            descriptor.Name("CatalogPriceInput");
            descriptor.Field(c => c.CatalogId).Name("id").Type<NonNullType<IdType>>();
            descriptor.Field(c => c.WholeSalePricePerUnit);
        }
    }
}