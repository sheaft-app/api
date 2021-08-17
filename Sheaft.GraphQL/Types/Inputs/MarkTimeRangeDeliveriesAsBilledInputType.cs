using HotChocolate.Types;
using Sheaft.Mediatr.Billing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class MarkTimeRangeDeliveriesAsBilledInputType : SheaftInputType<MarkTimeRangeDeliveriesAsBilledCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<MarkTimeRangeDeliveriesAsBilledCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("MarkTimeRangeDeliveriesAsBilledInput");

            descriptor
                .Field(c => c.From)
                .Name("from");
            
            descriptor
                .Field(c => c.To)
                .Name("to");
            
            descriptor
                .Field(c => c.Kinds)
                .Name("kinds");
        }
    }
}