using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ExpectedDeliveryDtoType : SheaftOutputType<ExpectedDeliveryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ExpectedDeliveryDto> descriptor)
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
            
            descriptor
                .Field(c => c.Day)
                .Name("day");
        }
    }
}
