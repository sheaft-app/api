using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateOwnerInputType : SheaftInputType<OwnerInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<OwnerInputDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateOwnerInput");
            
            descriptor
                .Field(c => c.FirstName)
                .Name("firstName");
                
            descriptor
                .Field(c => c.LastName)
                .Name("lastName");
                
            descriptor
                .Field(c => c.Email)
                .Name("email");
                
            descriptor
                .Field(c => c.BirthDate)
                .Name("birthDate");
                
            descriptor
                .Field(c => c.Nationality)
                .Name("nationality");
                
            descriptor
                .Field(c => c.CountryOfResidence)
                .Name("countryOfResidence");

            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<NonNullType<AddressInputType>>();
        }
    }
}
