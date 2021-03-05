using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SuggestAddressType : SheaftOutputType<SuggestAddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SuggestAddressDto> descriptor)
        {
            descriptor.Field(c => c.Zipcode);
            descriptor.Field(c => c.City);
        }
    }
}
