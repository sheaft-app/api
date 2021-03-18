using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SuggestProducerType : SheaftOutputType<SuggestProducerDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SuggestProducerDto> descriptor)
        {
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Address).Type<SuggestAddressType>();
        }
    }
}
