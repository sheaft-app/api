using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateDocumentType : SheaftInputType<CreateDocumentDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateDocumentDto> descriptor)
        {
            descriptor.Name("CreateDocumentInput");
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.LegalId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Kind)
                .Type<NonNullType<DocumentKindEnumType>>();
        }
    }
}
