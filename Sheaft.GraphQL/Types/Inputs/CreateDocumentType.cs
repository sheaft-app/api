using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;
using Sheaft.Mediatr.Document.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateDocumentType : SheaftInputType<CreateDocumentCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateDocumentCommand> descriptor)
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
