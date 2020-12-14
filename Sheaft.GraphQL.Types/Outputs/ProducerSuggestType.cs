using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class ProducerSuggestType : SheaftOutputType<ProducerSuggestDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducerSuggestDto> descriptor)
        {
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Address).Type<AddressSuggestType>();
        }
    }
}
