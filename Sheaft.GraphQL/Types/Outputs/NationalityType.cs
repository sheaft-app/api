using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class NationalityType : SheaftOutputType<NationalityDto>
    {
        protected override void Configure(IObjectTypeDescriptor<NationalityDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Code)
                .Type<NonNullType<StringType>>();
        }
    }
}
