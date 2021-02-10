using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class StoresSearchType : SheaftOutputType<StoresSearchDto>
    {
        protected override void Configure(IObjectTypeDescriptor<StoresSearchDto> descriptor)
        {
            descriptor.Field(c => c.Count);
            descriptor.Field(c => c.Stores).Type<ListType<SearchStoreType>>();
        }
    }
}
