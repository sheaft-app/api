using HotChocolate.Types;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RefuseAgreementsInputType : SheaftInputType<RefuseAgreementsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RefuseAgreementsCommand> descriptor)
        {
            descriptor.Name("RefuseAgreementsInput");
            descriptor.Field(c => c.AgreementIds)
                .Name("Ids")
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.Reason);
        }
    }
}