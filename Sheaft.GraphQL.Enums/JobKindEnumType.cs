using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class JobKindEnumType : EnumType<JobKind>
    {
        protected override void Configure(IEnumTypeDescriptor<JobKind> descriptor)
        {
            descriptor.Value(JobKind.CreateOrders).Name("CREATE_ORDERS");
            descriptor.Value(JobKind.CreatePickingFromOrders).Name("CREATE_PICKING_FROM_ORDERS");
            descriptor.Value(JobKind.ImportProducts).Name("IMPORT_PRODUCTS");
            descriptor.Value(JobKind.ExportUserData).Name("EXPORT_USER_DATA");
            descriptor.Value(JobKind.ExportProducts).Name("EXPORT_PRODUCTS");
            descriptor.Value(JobKind.OrderPicking).Name("ORDER_PICKING");
        }
    }
}
