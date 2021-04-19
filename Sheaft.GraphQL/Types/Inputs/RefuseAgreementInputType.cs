using HotChocolate.Types;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RefuseAgreementInputType : SheaftInputType<RefuseAgreementCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RefuseAgreementCommand> descriptor)
        {
            descriptor.Name("RefuseAgreementInput");
            descriptor.Field(c => c.AgreementId)
                .Name("Id")
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Reason);
        }
    }
}