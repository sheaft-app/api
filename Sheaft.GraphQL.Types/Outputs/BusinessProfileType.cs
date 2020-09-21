using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class BusinessProfileType : SheaftOutputType<BusinessProfileDto>
    {
        protected override void Configure(IObjectTypeDescriptor<BusinessProfileDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.VatIdentifier);
            descriptor.Field(c => c.Siret);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressType>>();
        }
    }
}
