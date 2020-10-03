using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class JobKindEnumType : EnumType<JobKind>
    {
        protected override void Configure(IEnumTypeDescriptor<JobKind> descriptor)
        {
            descriptor.Value(JobKind.ExportPickingOrders).Name("EXPORT_PICKING_ORDERS");
            descriptor.Value(JobKind.ImportProducts).Name("IMPORT_PRODUCTS");
            descriptor.Value(JobKind.ExportUserData).Name("EXPORT_USER_DATA");
        }
    }
}
