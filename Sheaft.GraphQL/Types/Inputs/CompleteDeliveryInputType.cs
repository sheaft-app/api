using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CompleteDeliveryInputType : SheaftInputType<CompleteDeliveryCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CompleteDeliveryCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CompleteDeliveryInput");

            descriptor
                .Field(c => c.DeliveryId)
                .ID(nameof(Delivery))
                .Name("id");

            descriptor
                .Field(c => c.ReceptionedBy)
                .Name("receptionedBy");

            descriptor
                .Field(c => c.Comment)
                .Name("comment");

            descriptor
                .Field(c => c.ReturnedProducts)
                .Name("returnedProducts")
                .Type<ListType<ReturnedProductDtoInputType>>();

            descriptor
                .Field(c => c.ReturnedReturnables)
                .Name("returnedReturnables")
                .Type<ListType<ReturnedReturnableDtoInputType>>();
        }
    }
}