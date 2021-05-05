using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ResetOrderInputType : SheaftInputType<ResetOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResetOrderCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ResetOrderInput");

            descriptor
                .Field(c => c.OrderId)
                .Name("id")
                .ID(nameof(Order));
        }
    }
}