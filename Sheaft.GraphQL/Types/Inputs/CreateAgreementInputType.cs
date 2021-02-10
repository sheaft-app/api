using HotChocolate.Types;
using Sheaft.GraphQL.Enums;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class CreateAgreementInputType : SheaftInputType<CreateAgreementInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateAgreementInput> descriptor)
        {
            descriptor.Field(c => c.DeliveryModeId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.SelectedHours)
                .Type<ListType<TimeSlotGroupInputType>>();

            descriptor.Field(c => c.StoreId)
                .Type<NonNullType<IdType>>();
        }
    }
}
