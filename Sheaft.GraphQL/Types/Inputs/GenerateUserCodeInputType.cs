using HotChocolate.Types;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class GenerateUserCodeInputType : SheaftInputType<GenerateUserCodeCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<GenerateUserCodeCommand> descriptor)
        {
            descriptor.Name("GenerateUserCodeInput");
            descriptor.Field(c => c.UserId)
                .Type<IdType>();
        }
    }
}