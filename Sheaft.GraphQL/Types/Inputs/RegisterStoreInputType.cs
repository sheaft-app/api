using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Store.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RegisterStoreInputType : SheaftInputType<RegisterStoreCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterStoreCommand> descriptor)
        {
            descriptor.Name("RegisterStoreInput");
            descriptor.Field(c => c.OpenForNewBusiness);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.SponsoringCode);

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<IdType>>();

            descriptor.Field(c => c.OpeningHours)
                .Type<ListType<TimeSlotGroupInputType>>();

            descriptor.Field(c => c.Legals)
                .Type<NonNullType<BusinessLegalInputType>>();
        }
    }
}
