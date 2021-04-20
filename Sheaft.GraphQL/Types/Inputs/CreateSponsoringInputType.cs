using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Sponsor.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateSponsoringInputType : SheaftInputType<CreateSponsoringCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateSponsoringCommand> descriptor)
        {
            descriptor.Name("CreateSponsoringInput");
            descriptor.Field(c => c.UserId)
                .Name("id")
                .Type<NonNullType<IdType>>();
        }
    }
}
