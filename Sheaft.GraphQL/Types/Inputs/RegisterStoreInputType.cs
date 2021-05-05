using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Store.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RegisterStoreInputType : SheaftInputType<RegisterStoreCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterStoreCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RegisterStoreInput");
            
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
                .Field(c => c.SponsoringCode)
                .Name("sponsoringCode");

            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<NonNullType<AddressInputType>>();

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
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Tags)
                .Name("tags")
                .ID(nameof(Tag));

            descriptor
                .Field(c => c.Legals)
                .Name("legals")
                .Type<NonNullType<BusinessLegalInputType>>();

            descriptor.Field(c => c.OpeningHours)
                .Name("openingHours")
                .Type<ListType<TimeSlotGroupInputType>>();
        }
    }
}
