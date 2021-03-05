using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DonationType : SheaftOutputType<DonationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DonationDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.Fees);
            descriptor.Field(c => c.Debited);
            descriptor.Field(c => c.Reference);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.ExecutedOn);

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<TransactionKindEnumType>>();

            descriptor.Field(c => c.Status)
                .Type<NonNullType<TransactionStatusEnumType>>();
            
            descriptor.Field(c => c.CreditedUser)
                .Type<NonNullType<UserType>>();
        }
    }
}