using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CancelAgreementsInputType : SheaftInputType<CancelAgreementsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CancelAgreementsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CancelAgreementsInput");
            
            descriptor
                .Field(c => c.AgreementIds)
                .Name("ids")
                .ID(nameof(Agreement));

            descriptor
                .Field(c => c.Reason)
                .Name("reason");
        }
    }
}