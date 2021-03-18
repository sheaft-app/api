using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ExportTransactionsInputType : SheaftInputType<ExportTransactionsDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ExportTransactionsDto> descriptor)
        {
            descriptor.Name("ExportTransactionsInput");
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
}