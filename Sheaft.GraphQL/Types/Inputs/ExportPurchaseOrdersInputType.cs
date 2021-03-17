using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ExportPurchaseOrdersInputType : SheaftInputType<ExportPurchaseOrdersInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ExportPurchaseOrdersInput> descriptor)
        {
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
}