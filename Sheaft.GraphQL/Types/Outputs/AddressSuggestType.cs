using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class AddressSuggestType : SheaftOutputType<AddressSuggestDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AddressSuggestDto> descriptor)
        {
            descriptor.Field(c => c.Zipcode);
            descriptor.Field(c => c.City);
        }
    }
}
