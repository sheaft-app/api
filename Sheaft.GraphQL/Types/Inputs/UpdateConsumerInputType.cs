using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateConsumerInputType : SheaftInputType<UpdateConsumerInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateConsumerInput> descriptor)
        {
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}
