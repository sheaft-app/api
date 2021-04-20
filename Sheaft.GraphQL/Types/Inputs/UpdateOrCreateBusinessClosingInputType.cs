using HotChocolate.Types;
using Sheaft.Mediatr.BusinessClosing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateBusinessClosingInputType : SheaftInputType<UpdateOrCreateBusinessClosingCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateBusinessClosingCommand> descriptor)
        {
            descriptor.Name("UpdateOrCreateBusinessClosingInput");
            descriptor.Field(c => c.UserId)
                .Name("id")
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Closing)
                .Type<NonNullType<UpdateOrCreateClosingInputType>>();
        }
    }
}