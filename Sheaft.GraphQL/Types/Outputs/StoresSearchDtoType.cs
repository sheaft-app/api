using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class StoresSearchDtoType : SheaftOutputType<StoresSearchDto>
    {
        protected override void Configure(IObjectTypeDescriptor<StoresSearchDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Count)
                .Name("count");
            
            descriptor
                .Field(c => c.Stores)
                .Name("stores")
                .Type<ListType<SearchStoreDtoType>>();
        }
    }
}
