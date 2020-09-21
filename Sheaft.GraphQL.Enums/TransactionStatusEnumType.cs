using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class TransactionStatusEnumType : EnumType<TransactionStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<TransactionStatus> descriptor)
        {
            descriptor.Value(TransactionStatus.NotSpecified).Name("NOT_SPECIFIED");
            descriptor.Value(TransactionStatus.Created).Name("CREATED");
            descriptor.Value(TransactionStatus.Failed).Name("FAILED");
            descriptor.Value(TransactionStatus.Succeeded).Name("SUCCEEDED");
            descriptor.Value(TransactionStatus.Waiting).Name("WAITING");
        }
    }
}
