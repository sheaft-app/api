using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class ProducerExpectedDeliveryInputType : SheaftInputType<ProducerExpectedDeliveryInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProducerExpectedDeliveryInput> descriptor)
        {
            descriptor.Field(c => c.ProducerId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.ExpectedDeliveryDate);
            descriptor.Field(c => c.Comment);
            descriptor.Field(c => c.DeliveryModeId).Type<NonNullType<IdType>>();
        }
    }
}
