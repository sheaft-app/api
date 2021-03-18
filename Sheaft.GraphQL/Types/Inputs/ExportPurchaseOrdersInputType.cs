using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ExportPurchaseOrdersInputType : SheaftInputType<ExportPurchaseOrdersDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ExportPurchaseOrdersDto> descriptor)
        {
            descriptor.Name("ExportPurchaseOrdersInput");
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
}