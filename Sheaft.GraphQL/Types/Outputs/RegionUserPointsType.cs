using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class RegionUserPointsType : SheaftOutputType<RegionUserPointsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RegionUserPointsDto> descriptor)
        {
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);
            descriptor.Field(c => c.RegionId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.UserId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Kind).Type<NonNullType<ProfileKindEnumType>>();
            descriptor.Field(c => c.Picture);
        }
    }
}
