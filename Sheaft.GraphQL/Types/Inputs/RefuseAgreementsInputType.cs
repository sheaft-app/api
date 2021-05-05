using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RefuseAgreementsInputType : SheaftInputType<RefuseAgreementsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RefuseAgreementsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RefuseAgreementsInput");
            
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