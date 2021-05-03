using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SuggestProducerDtoType : SheaftOutputType<SuggestProducerDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SuggestProducerDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .Field(c => c.Id)
                .Name("id");
                
            descriptor
                .Field(c => c.Name)
                .Name("name");
                
            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<SuggestAddressDtoType>();
        }
    }
}
