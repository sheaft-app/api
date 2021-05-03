using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ExpectedOrderDeliveryType : SheaftOutputType<ExpectedOrderDelivery>
    {
        protected override void Configure(IObjectTypeDescriptor<ExpectedOrderDelivery> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.ExpectedDeliveryDate)
                .Name("expectedDeliveryDate");
                
            descriptor
                .Field(c => c.From)
                .Name("from");
                
            descriptor
                .Field(c => c.To)
                .Name("to");
        }
    }
}
