using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.QuickOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetQuickOrderAsDefaultInputType : SheaftInputType<SetQuickOrderAsDefaultCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetQuickOrderAsDefaultCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SetQuickOrderAsDefaultInput");

            descriptor
                .Field(c => c.QuickOrderId)
                .Name("id")
                .ID(nameof(QuickOrder));
        }
    }
}