using HotChocolate.Types;
using Sheaft.GraphQL.Enums;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
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
