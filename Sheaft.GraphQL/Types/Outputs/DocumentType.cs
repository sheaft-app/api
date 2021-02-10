using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DocumentType : SheaftOutputType<DocumentDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DocumentDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.ReasonCode);
            descriptor.Field(c => c.ReasonMessage);

            descriptor.Field(c => c.Status)
                .Type<NonNullType<DocumentStatusEnumType>>();

            descriptor.Field(c => c.Pages)
                .Type<ListType<PageType>>();
        }
    }
}
