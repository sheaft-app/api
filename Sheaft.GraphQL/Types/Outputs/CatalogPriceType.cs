using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CatalogPriceType : SheaftOutputType<CatalogProductDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CatalogProductDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.VatPricePerUnit);
            descriptor.Field(c => c.OnSalePricePerUnit);
            descriptor.Field(c => c.WholeSalePricePerUnit);
            descriptor.Field(c => c.OnSalePrice);
            descriptor.Field(c => c.WholeSalePrice);
            descriptor.Field(c => c.VatPrice);
        }
    }
}