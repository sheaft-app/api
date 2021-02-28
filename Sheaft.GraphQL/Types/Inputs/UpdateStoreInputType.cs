using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateStoreInputType : SheaftInputType<UpdateStoreInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateStoreInput> descriptor)
        {
            descriptor.Field(c => c.OpenForNewBusiness);
            descriptor.Field(c => c.OpeningHours);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Summary);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Facebook);
            descriptor.Field(c => c.Twitter);
            descriptor.Field(c => c.Instagram);
            descriptor.Field(c => c.Website);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<FullAddressInputType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<IdType>>();

            descriptor.Field(c => c.OpeningHours)
                .Type<ListType<TimeSlotGroupInputType>>();
        }
    }
}
