using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Security;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class OrderType : SheaftOutputType<OrderDto>
    {
        protected override void Configure(IObjectTypeDescriptor<OrderDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.DonationKind);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Reference);

            descriptor.Field(c => c.TotalWholeSalePrice);
            descriptor.Field(c => c.TotalVatPrice);
            descriptor.Field(c => c.TotalOnSalePrice);
            descriptor.Field(c => c.TotalReturnableWholeSalePrice);
            descriptor.Field(c => c.TotalReturnableVatPrice);
            descriptor.Field(c => c.TotalReturnableOnSalePrice);
            descriptor.Field(c => c.TotalProductWholeSalePrice);
            descriptor.Field(c => c.TotalProductVatPrice);
            descriptor.Field(c => c.TotalProductOnSalePrice);
            descriptor.Field(c => c.TotalPrice);

            descriptor.Field(c => c.Donation);

            descriptor.Field(c => c.FeesPrice);
            descriptor.Field(c => c.InternalFeesPrice);
            descriptor.Field(c => c.TotalFees);

            descriptor.Field(c => c.FeesFixedAmount);
            descriptor.Field(c => c.FeesPercent);

            descriptor.Field(c => c.ReturnablesCount);
            descriptor.Field(c => c.LinesCount);
            descriptor.Field(c => c.ProductsCount);

            descriptor.Field(c => c.TotalWeight);

            descriptor.Field(c => c.User)
                .Type<UserType>();

            descriptor.Field(c => c.Products)
                .Type<ListType<OrderProductType>>();
            
            descriptor.Field(c => c.Deliveries)
                .Type<ListType<OrderDeliveryType>>();
        }
    }
}
