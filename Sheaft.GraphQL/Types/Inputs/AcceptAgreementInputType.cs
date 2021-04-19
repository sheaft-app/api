using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AcceptAgreementInputType : SheaftInputType<AcceptAgreementCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AcceptAgreementCommand> descriptor)
        {
            descriptor.Name("AcceptAgreementInput");
            descriptor.Field(c => c.AgreementId)
                .Name("Id")
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.SelectedHours)
                .Type<ListType<TimeSlotGroupInputType>>();

            descriptor.Field(c => c.CatalogId)
                .Type<IdType>();
        }
    }
}