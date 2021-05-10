using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Business.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateOrCreateBusinessClosingInputType : SheaftInputType<UpdateOrCreateBusinessClosingCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateBusinessClosingCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateOrCreateBusinessClosingInput");
            
            descriptor
                .Field(c => c.UserId)
                .Name("id")
                .ID(nameof(User));
            
            descriptor
                .Field(c => c.Closing)
                .Name("closing")
                .Type<NonNullType<BusinessClosingInputType>>();
        }
    }
}