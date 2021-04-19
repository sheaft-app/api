using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ProducerExpectedDeliveryInputType : SheaftInputType<ProducerExpectedDeliveryInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProducerExpectedDeliveryInputDto> descriptor)
        {
            descriptor.Name("ProducerExpectedDeliveryInput");
            descriptor.Field(c => c.ProducerId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.ExpectedDeliveryDate);
            descriptor.Field(c => c.Comment);
            descriptor.Field(c => c.DeliveryModeId).Type<NonNullType<IdType>>();
        }
    }
}
