using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class OwnerType : SheaftOutputType<Owner>
    {
        protected override void Configure(IObjectTypeDescriptor<Owner> descriptor)
        {
            base.Configure(descriptor);
            
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
                .Field(c => c.CountryOfResidence)
                .Name("countryOfResidence");
                
            descriptor
                .Field(c => c.Nationality)
                .Name("nationality");
                
            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<OwnerAddressType>();
        }
    }
}
