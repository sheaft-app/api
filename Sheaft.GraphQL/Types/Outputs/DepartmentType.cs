using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class DepartmentType : SheaftOutputType<DepartmentDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DepartmentDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Code)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Level)
                .Type<LevelType>();
        }
    }
}
