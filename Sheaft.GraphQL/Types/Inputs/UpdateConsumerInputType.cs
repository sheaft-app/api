using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Consumer.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateConsumerInputType : SheaftInputType<UpdateConsumerCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateConsumerCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateConsumerInput");
            
            descriptor
                .Field(c => c.Phone)
                .Name("phone");
            
            descriptor
                .Field(c => c.Picture)
                .Name("picture");
            
            descriptor
                .Field(c => c.Summary)
                .Name("summary");
            
            descriptor
                .Field(c => c.Description)
                .Name("description");
            
            descriptor
                .Field(c => c.Facebook)
                .Name("facebook");
            
            descriptor
                .Field(c => c.Twitter)
                .Name("twitter");
            
            descriptor
                .Field(c => c.Instagram)
                .Name("instagram");
            
            descriptor
                .Field(c => c.Website)
                .Name("website");

            descriptor
                .Field(c => c.Email)
                .Name("email")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.FirstName)
                .Name("firstName")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.LastName)
                .Name("lastName")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.ConsumerId)
                .Name("id")
                .ID(nameof(Consumer));

            descriptor.Field(c => c.Pictures)
                .Name("pictures")
                .Type<ListType<ProfilePictureInputType>>();
        }
    }
}
