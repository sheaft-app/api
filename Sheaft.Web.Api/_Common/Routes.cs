namespace Sheaft.Web.Api;

public static class Routes
{
    public const string API = "api";
    public const string PASSWORD = $"{API}/password";
    public const string TOKEN = $"{API}/token";
    public const string ACCOUNT = $"{API}/account";
    public const string SUPPLIERS = $"{API}/suppliers";
    public const string SUPPLIER_PRODUCTS = $"{API}/suppliers/{{supplierId}}/products";
    public const string SUPPLIER_RETURNABLES = $"{API}/suppliers/{{supplierId}}/returnables";
    public const string SUPPLIER_BATCHES = $"{API}/suppliers/{{supplierId}}/batches";
    public const string CUSTOMERS = $"{API}/customers";
    public const string AGREEMENTS = $"{API}/agreements";
    public const string ORDERS = $"{API}/orders";
    public const string DELIVERIES = $"{API}/deliveries";
    public const string INVOICES = $"{API}/invoices";
    public const string DOCUMENTS = $"{API}/documents";
}