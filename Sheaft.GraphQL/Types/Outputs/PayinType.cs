using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PayinType : SheaftOutputType<PayinDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PayinDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.ResultCode);
            descriptor.Field(c => c.ResultMessage);
            descriptor.Field(c => c.RedirectUrl);
            descriptor.Field(c => c.CreditedUser);
            descriptor.Field(c => c.Credited);
            descriptor.Field(c => c.Debited);
            descriptor.Field(c => c.Fees);
            descriptor.Field(c => c.ExecutedOn);
            descriptor.Field(c => c.Reference);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            
            descriptor.Field(c => c.Author)
                .Type<NonNullType<UserType>>();
            
            descriptor.Field(c => c.Kind)
                .Type<NonNullType<TransactionKindEnumType>>();

            descriptor.Field(c => c.Status)
                .Type<NonNullType<TransactionStatusEnumType>>();
        }
    }
}
