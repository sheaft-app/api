using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Consumer.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RegisterConsumerInputType : SheaftInputType<RegisterConsumerCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterConsumerCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RegisterConsumerInput");
            
            descriptor
                .Field(c => c.Phone)
                .Name("phone");
                
            descriptor
                .Field(c => c.Picture)
                .Name("picture");
                
            descriptor
                .Field(c => c.SponsoringCode)
                .Name("sponsoringCode");
                
            descriptor.Field(c => c.Email)
                .Name("email")
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Name("firstName")
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Name("lastName")
                .Type<NonNullType<StringType>>();
        }
    }
}
