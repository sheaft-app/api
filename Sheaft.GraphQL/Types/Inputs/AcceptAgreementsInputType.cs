using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AcceptAgreementsInputType : SheaftInputType<AcceptAgreementsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AcceptAgreementsCommand> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("AcceptAgreementsInput");
            
            descriptor
                .Field(c => c.AgreementIds)
                .Name("ids")
                .ID(nameof(Agreement));

            descriptor
                .Field(c => c.DeliveryId)
                .Name("deliveryId")
                .ID(nameof(DeliveryMode));

            descriptor
                .Field(c => c.CatalogId)
                .Name("catalogId")
                .ID(nameof(Catalog));
        }
    }
}