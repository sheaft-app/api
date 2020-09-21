using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class CountryPointsType : SheaftOutputType<CountryPointsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CountryPointsDto> descriptor)
        {
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Users);
        }
    }
}
