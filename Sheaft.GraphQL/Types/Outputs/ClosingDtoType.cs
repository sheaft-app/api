using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ClosingDtoType : SheaftOutputType<ClosingDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ClosingDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Field(c => c.Id)
                .Name("id")
                .Type<NonNullType<IdType>>();
            
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