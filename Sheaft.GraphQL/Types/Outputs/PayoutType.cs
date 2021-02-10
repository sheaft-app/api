using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PayoutType : SheaftOutputType<PayoutDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PayoutDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.Fees);
            descriptor.Field(c => c.Debited);
            descriptor.Field(c => c.Identifier);
            descriptor.Field(c => c.Reference);
            descriptor.Field(c => c.ResultCode);
            descriptor.Field(c => c.ResultMessage);

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<TransactionKindEnumType>>();

            descriptor.Field(c => c.Status)
                .Type<NonNullType<TransactionStatusEnumType>>();

            descriptor.Field(c => c.DebitedUser)
                .Type<NonNullType<UserProfileType>>();
        }
    }
}
