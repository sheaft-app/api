using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateProducerInputType : SheaftInputType<UpdateProducerCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateProducerCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateProducerInput");
            
            descriptor
                .Field(c => c.OpenForNewBusiness)
                .Name("openForNewBusiness");
                
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
                .Field(c => c.ProducerId)
                .Name("id")
                .ID(nameof(Producer));

            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<NonNullType<AddressInputType>>();

            descriptor
                .Field(c => c.Email)
                .Name("email")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.FirstName)
                .Name("firstName")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.LastName)
                .Name("lastName")
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Pictures)
                .Name("pictures")
                .Type<ListType<ProfilePictureInputType>>();
        }
    }
}
