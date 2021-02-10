using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateDocumentType : SheaftInputType<CreateDocumentInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateDocumentInput> descriptor)
        {
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.UserId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<DocumentKindEnumType>>();
        }
    }
}
