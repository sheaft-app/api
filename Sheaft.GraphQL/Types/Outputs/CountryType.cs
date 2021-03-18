using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CountryType : SheaftOutputType<CountryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CountryDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Code)
                .Type<NonNullType<StringType>>();
        }
    }
}
