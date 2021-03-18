using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class GenerateUserSponsoringCodeInputType : SheaftInputType<GenerateUserSponsoringCodeDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<GenerateUserSponsoringCodeDto> descriptor)
        {
            descriptor.Name("GenerateUserSponsoringCodeInput");
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}
