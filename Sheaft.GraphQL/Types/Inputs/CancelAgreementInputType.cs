using HotChocolate.Types;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CancelAgreementsInputType : SheaftInputType<CancelAgreementsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CancelAgreementsCommand> descriptor)
        {
            descriptor.Name("CancelAgreementsInput");
            descriptor.Field(c => c.AgreementIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.Reason);
        }
    }
}