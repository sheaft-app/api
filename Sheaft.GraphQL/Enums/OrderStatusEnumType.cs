using HotChocolate.Types;
using System;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class OrderStatusEnumType : EnumType<OrderStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<OrderStatus> descriptor)
        {
            descriptor.Value(OrderStatus.Created).Name("CREATED");
            descriptor.Value(OrderStatus.Waiting).Name("WAITING");
            descriptor.Value(OrderStatus.Validated).Name("VALIDATED");
            descriptor.Value(OrderStatus.Refused).Name("REFUSED");
            descriptor.Value(OrderStatus.Archived).Name("ARCHIVED");
            descriptor.Value(OrderStatus.Expired).Name("EXPIRED");
        }
    }
}
