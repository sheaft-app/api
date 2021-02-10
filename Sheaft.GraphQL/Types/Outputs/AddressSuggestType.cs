using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
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
