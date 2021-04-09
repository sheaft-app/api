using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AcceptAgreementInputType : SheaftInputType<AcceptAgreementDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AcceptAgreementDto> descriptor)
        {
            descriptor.Name("AcceptAgreementInput");
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.SelectedHours)
                .Type<ListType<TimeSlotGroupInputType>>();

            descriptor.Field(c => c.CatalogId)
                .Type<IdType>();
        }
    }
}