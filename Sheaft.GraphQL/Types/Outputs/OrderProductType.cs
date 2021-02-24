using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class OrderProductType : SheaftOutputType<OrderProductDto>
    {
        protected override void Configure(IObjectTypeDescriptor<OrderProductDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Quantity);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.TotalWeight);
            descriptor.Field(c => c.UnitOnSalePrice);
            descriptor.Field(c => c.UnitVatPrice);
            descriptor.Field(c => c.UnitWholeSalePrice);
            descriptor.Field(c => c.UnitVatPrice);
            descriptor.Field(c => c.UnitWeight);

            descriptor.Field(c => c.TotalWholeSalePrice);
            descriptor.Field(c => c.TotalVatPrice);
            descriptor.Field(c => c.TotalOnSalePrice);
            descriptor.Field(c => c.TotalReturnableWholeSalePrice);
            descriptor.Field(c => c.TotalReturnableVatPrice);
            descriptor.Field(c => c.TotalReturnableOnSalePrice);
            descriptor.Field(c => c.TotalProductWholeSalePrice);
            descriptor.Field(c => c.TotalProductVatPrice);
            descriptor.Field(c => c.TotalProductOnSalePrice);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();
            
            descriptor.Field(c => c.Producer)
                .Type<NonNullType<UserProfileType>>();
        }
    }
}