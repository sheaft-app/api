using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
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
