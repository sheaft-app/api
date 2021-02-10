using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class RegionPointsType : SheaftOutputType<RegionPointsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RegionPointsDto> descriptor)
        {
            descriptor.Field(c => c.RegionName);
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);
            descriptor.Field(c => c.RegionId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Users);
        }
    }
}
