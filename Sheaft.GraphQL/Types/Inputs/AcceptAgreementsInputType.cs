using HotChocolate.Types;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AcceptAgreementsInputType : SheaftInputType<AcceptAgreementsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AcceptAgreementsCommand> descriptor)
        {
            descriptor.Name("AcceptAgreementsInput");
            descriptor.Field(c => c.AgreementIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.DeliveryId)
                .Type<IdType>();

            descriptor.Field(c => c.CatalogId)
                .Type<IdType>();
        }
    }
}