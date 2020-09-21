using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class DocumentStatusEnumType : EnumType<DocumentStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<DocumentStatus> descriptor)
        {
            descriptor.Value(DocumentStatus.NotSpecified).Name("NOT_SPECIFIED");
            descriptor.Value(DocumentStatus.Created).Name("CREATED");
            descriptor.Value(DocumentStatus.OutOfDate).Name("OUT_OF_DATE");
            descriptor.Value(DocumentStatus.Refused).Name("REFUSED");
            descriptor.Value(DocumentStatus.Validated).Name("VALIDATED");
            descriptor.Value(DocumentStatus.ValidationAsked).Name("VALIDATION_ASKED");
            descriptor.Value(DocumentStatus.WaitingForCreation).Name("WAITING_FOR_CREATION");
            descriptor.Value(DocumentStatus.WaitingForFirstOrder).Name("WAITING_FOR_FIRST_ORDER");
        }
    }
}
