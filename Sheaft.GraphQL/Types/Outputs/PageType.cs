using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PageType : SheaftOutputType<PageDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PageDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.FileName);
            descriptor.Field(c => c.Extension);
            descriptor.Field(c => c.Size);
        }
    }
}
