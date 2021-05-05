using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class BusinessClosingInputType : SheaftInputType<ClosingInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ClosingInputDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("BusinessClosingInput");

            descriptor
                .Field(c => c.Id)
                .Name("id")
                .ID(nameof(BusinessClosing));
                
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