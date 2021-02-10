using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class ExpectedOrderDeliveryType : ObjectType<ExpectedOrderDeliveryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ExpectedOrderDeliveryDto> descriptor)
        {
            descriptor.Field(c => c.ExpectedDeliveryDate);
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
}
