using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Legal.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateConsumerLegalsInputType : SheaftInputType<UpdateConsumerLegalCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateConsumerLegalCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateConsumerLegalInput");
            
            descriptor
                .Field(c => c.LegalId)
                .Name("id")
                .ID(nameof(ConsumerLegal));

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
