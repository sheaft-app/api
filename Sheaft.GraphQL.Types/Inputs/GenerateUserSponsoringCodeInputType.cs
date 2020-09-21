using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class GenerateUserSponsoringCodeInputType : SheaftInputType<GenerateUserSponsoringCodeInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<GenerateUserSponsoringCodeInput> descriptor)
        {
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}
