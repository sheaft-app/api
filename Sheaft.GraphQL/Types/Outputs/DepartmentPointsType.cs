using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
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
