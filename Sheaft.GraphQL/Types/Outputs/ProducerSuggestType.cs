using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
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
