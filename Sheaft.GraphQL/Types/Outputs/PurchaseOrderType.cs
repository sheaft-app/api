using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Filters;

namespace Sheaft.GraphQL.Types
{
    public class PurchaseOrderType : SheaftOutputType<PurchaseOrderDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PurchaseOrderDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.TotalWeight);

            descriptor.Field(c => c.TotalWholeSalePrice);
            descriptor.Field(c => c.TotalVatPrice);
            descriptor.Field(c => c.TotalOnSalePrice);
            descriptor.Field(c => c.TotalReturnableWholeSalePrice);
            descriptor.Field(c => c.TotalReturnableVatPrice);
            descriptor.Field(c => c.TotalReturnableOnSalePrice);
            descriptor.Field(c => c.TotalProductWholeSalePrice);
            descriptor.Field(c => c.TotalProductVatPrice);
            descriptor.Field(c => c.TotalProductOnSalePrice);

            descriptor.Field(c => c.ReturnablesCount);
            descriptor.Field(c => c.LinesCount);
            descriptor.Field(c => c.ProductsCount);

            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Reason);
            descriptor.Field(c => c.Comment);

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.ExpectedDelivery)
                .Type<NonNullType<ExpectedPurchaseOrderDeliveryType>>();

            descriptor.Field(c => c.Sender)
                .Type<NonNullType<UserProfileType>>();

            descriptor.Field(c => c.Vendor)
                .Type<NonNullType<UserProfileType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<PurchaseOrderProductQuantityType>>>()
                .UseFiltering<PurchaseOrderProductQuantityFilterType>();
        }
    }
}
