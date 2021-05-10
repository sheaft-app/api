using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ProducerExpectedDeliveryInputType : SheaftInputType<ProducerExpectedDeliveryInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProducerExpectedDeliveryInputDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ProducerExpectedDeliveryInput");
            
            descriptor
                .Field(c => c.ProducerId)
                .Name("producerId")
                .ID(nameof(Producer));
            
            descriptor
                .Field(c => c.DeliveryModeId)
                .Name("deliveryModeId")
                .ID(nameof(DeliveryMode));
            
            descriptor
                .Field(c => c.ExpectedDeliveryDate)
                .Name("expectedDeliveryDate");
            
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
        }
    }
}
