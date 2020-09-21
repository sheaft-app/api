using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class ProductType : SheaftOutputType<ProductDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.VatPricePerUnit);
            descriptor.Field(c => c.OnSalePricePerUnit);
            descriptor.Field(c => c.WholeSalePricePerUnit);
            descriptor.Field(c => c.OnSalePrice);
            descriptor.Field(c => c.WholeSalePrice);
            descriptor.Field(c => c.VatPrice);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.RatingsCount);
            descriptor.Field(c => c.QuantityPerUnit);
            descriptor.Field(c => c.Unit);
            descriptor.Field(c => c.Conditioning);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Weight);
            descriptor.Field(c => c.Rating);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.ImageLarge);
            descriptor.Field(c => c.ImageMedium);
            descriptor.Field(c => c.ImageSmall);
            descriptor.Field(c => c.IsReturnable);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Returnable)
                .Type<ReturnableType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<BusinessProfileType>>();
        }
    }
}
