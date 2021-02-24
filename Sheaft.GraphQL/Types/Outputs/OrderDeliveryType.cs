using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class OrderDeliveryType : SheaftOutputType<OrderDeliveryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<OrderDeliveryDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Comment);

            descriptor.Field(c => c.DeliveryMode)
                .Type<NonNullType<DeliveryModeType>>();

            descriptor.Field(c => c.ExpectedDelivery)
                .Type<NonNullType<ExpectedOrderDeliveryType>>();
        }
    }
}