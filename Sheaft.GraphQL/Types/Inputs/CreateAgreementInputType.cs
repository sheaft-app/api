using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateAgreementInputType : SheaftInputType<CreateAgreementCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateAgreementCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateAgreementInput");

            descriptor
                .Field(c => c.DeliveryId)
                .Name("deliveryId")
                .ID(nameof(DeliveryMode));

            descriptor
                .Field(c => c.StoreId)
                .Name("storeId")
                .ID(nameof(Store));
            
            descriptor
                .Field(c => c.ProducerId)
                .Name("producerId")
                .ID(nameof(Producer));

            descriptor
                .Field(c => c.CatalogId)
                .Name("catalogId")
                .ID(nameof(Catalog));
        }
    }
}
