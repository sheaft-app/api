using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
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
