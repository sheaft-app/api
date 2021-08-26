using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateSponsoringInputType : SheaftInputType<CreateSponsoringCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateSponsoringCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateSponsoringInput");
            
            descriptor
                .Field(c => c.UserId)
                .Name("id")
                .ID(nameof(User));
        }
    }
}
