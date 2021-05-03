using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SuggestAddressDtoType : SheaftOutputType<SuggestAddressDto>
    {
        protected override void Configure(IObjectTypeDescriptor<SuggestAddressDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .Field(c => c.Zipcode)
                .Name("zipcode");
            
            descriptor
                .Field(c => c.City)
                .Name("city");
            
        }
    }
}
