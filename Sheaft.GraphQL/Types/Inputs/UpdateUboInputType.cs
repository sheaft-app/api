using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class UpdateUboInputType : SheaftInputType<UpdateUboInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateUboInput> descriptor)
        {
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.BirthDate);
            descriptor.Field(c => c.Nationality);

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();

            descriptor.Field(c => c.BirthPlace)
                .Type<NonNullType<BirthAddressInputType>>();
        }
    }
}
