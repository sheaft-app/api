using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
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
