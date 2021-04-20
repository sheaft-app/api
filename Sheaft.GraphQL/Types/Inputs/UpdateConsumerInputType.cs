using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Consumer.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateConsumerInputType : SheaftInputType<UpdateConsumerCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateConsumerCommand> descriptor)
        {
            descriptor.Name("UpdateConsumerInput");
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Summary);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Facebook);
            descriptor.Field(c => c.Twitter);
            descriptor.Field(c => c.Instagram);
            descriptor.Field(c => c.Website);

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.ConsumerId)
                .Name("id")
                .Type<NonNullType<IdType>>();
        }
    }
}
