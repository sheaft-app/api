using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Store.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateStoreInputType : SheaftInputType<UpdateStoreCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateStoreCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateStoreInput");
            
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
                .Field(c => c.StoreId)
                .Name("id")
                .ID(nameof(Store));

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

            descriptor
                .Field(c => c.Tags)
                .Name("tags")
                .ID(nameof(Tag));

            descriptor
                .Field(c => c.OpeningHours)
                .Name("openingHours")
                .Type<ListType<TimeSlotGroupInputType>>();

            descriptor.Field(c => c.Pictures)
                .Name("pictures")
                .Type<ListType<PictureInputType>>();
        }
    }
}
