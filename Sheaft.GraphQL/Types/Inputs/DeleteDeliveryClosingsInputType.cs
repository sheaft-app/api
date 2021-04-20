using HotChocolate.Types;
using Sheaft.Mediatr.DeliveryClosing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteDeliveryClosingsInputType : SheaftInputType<DeleteDeliveryClosingsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteDeliveryClosingsCommand> descriptor)
        {
            descriptor.Name("DeleteDeliveryClosingsInput");
            descriptor.Field(c => c.ClosingIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}