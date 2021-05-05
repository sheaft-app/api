using HotChocolate.Types;
using Sheaft.Application.Models;

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
                .Type<NonNullType<IdType>>();

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
