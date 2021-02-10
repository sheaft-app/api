using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
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
