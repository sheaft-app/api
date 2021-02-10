using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class DepartmentPointsType : SheaftOutputType<DepartmentPointsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DepartmentPointsDto> descriptor)
        {
            descriptor.Field(c => c.DepartmentName);
            descriptor.Field(c => c.RegionName);
            descriptor.Field(c => c.Code);
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);
            descriptor.Field(c => c.DepartmentId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.RegionId).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Users);
        }
    }
}
