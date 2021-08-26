using HotChocolate.Types;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RegisterProducerInputType : SheaftInputType<RegisterProducerCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterProducerCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RegisterProducerInput");
            
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
                .Field(c => c.NotSubjectToVat)
                .Name("notSubjectToVat")
                .Type<NonNullType<BooleanType>>();

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
                .Field(c => c.Legals)
                .Name("legals")
                .Type<NonNullType<BusinessLegalInputType>>();
        }
    }
}
