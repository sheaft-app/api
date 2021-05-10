using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeliveryClosingInputType : SheaftInputType<ClosingInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ClosingInputDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeliveryClosingInput");

            descriptor
                .Field(c => c.Id)
                .Name("id")
                .ID(nameof(DeliveryClosing));
                
            descriptor
                .Field(c => c.From)
                .Name("from");
                
            descriptor
                .Field(c => c.To)
                .Name("to");
                
            descriptor
                .Field(c => c.Reason)
                .Name("reason");
                
        }
    }
}