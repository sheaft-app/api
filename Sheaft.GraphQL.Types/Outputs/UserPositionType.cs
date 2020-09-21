using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Sheaft.Application.Models;
using Sheaft.Application.Queries;
using Sheaft.GraphQL.Enums;
using Sheaft.GraphQL.Filters;
using Sheaft.GraphQL.Sorts;

namespace Sheaft.GraphQL.Types
{
    public class UserPositionType : SheaftOutputType<UserPositionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserPositionDto> descriptor)
        {
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);
        }
    }
}
