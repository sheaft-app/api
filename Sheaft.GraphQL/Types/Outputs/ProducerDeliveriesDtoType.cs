using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProducerDeliveriesDtoType : SheaftOutputType<ProducerDeliveriesDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducerDeliveriesDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ProducerDeliveries");
            
            descriptor
                .Field(c => c.Id)
                .Name("id")
                .ID(nameof(Producer));

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();
            
            descriptor
                .Field(c => c.Deliveries)
                .Name("deliveries")
                .Type<ListType<DeliveryDtoType>>();

        }
    }
}
