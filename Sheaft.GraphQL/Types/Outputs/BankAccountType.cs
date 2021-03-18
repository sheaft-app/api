using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BankAccountType : SheaftOutputType<BankAccountDto>
    {
        protected override void Configure(IObjectTypeDescriptor<BankAccountDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.IBAN).Name("iban");
            descriptor.Field(c => c.BIC).Name("bic");
            descriptor.Field(c => c.Owner);
            descriptor.Field(c => c.Line1);
            descriptor.Field(c => c.Line2);
            descriptor.Field(c => c.Zipcode);
            descriptor.Field(c => c.City);
            descriptor.Field(c => c.Country);
        }
    }
}