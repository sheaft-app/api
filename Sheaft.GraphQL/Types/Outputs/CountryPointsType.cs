using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
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
