using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class RegionType : SheaftOutputType<RegionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RegionDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Code)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
