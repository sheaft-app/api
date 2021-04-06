using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateAgreementInputType : SheaftInputType<CreateAgreementDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateAgreementDto> descriptor)
        {
            descriptor.Name("CreateAgreementInput");
            descriptor.Field(c => c.DeliveryModeId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.SelectedHours)
                .Type<ListType<TimeSlotGroupInputType>>();

            descriptor.Field(c => c.StoreId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.CatalogId)
                .Type<IdType>();
        }
    }
}
