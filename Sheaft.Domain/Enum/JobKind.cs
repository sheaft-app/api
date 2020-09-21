namespace Sheaft.Domain.Enums
{
    public enum JobKind
    {
        CreateOrders = 100,
        CreatePickingFromOrders = 150,
        ImportProducts = 200,
        ExportProducts,
        OrderPicking = 300,
        ExportUserData = 400,
    }
}