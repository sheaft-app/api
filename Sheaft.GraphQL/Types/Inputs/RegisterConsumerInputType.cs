using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Consumer.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RegisterConsumerInputType : SheaftInputType<RegisterConsumerCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterConsumerCommand> descriptor)
        {
            descriptor.Name("RegisterConsumerInput");
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.SponsoringCode);

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();
        }
    }
}
