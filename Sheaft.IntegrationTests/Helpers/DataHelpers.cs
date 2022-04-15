using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.IntegrationTests.Helpers;

internal static class DataHelpers
{
    public static Siret GetDefaultSiret()
    {
        return new Siret("15932477173006");
    }
    
    public static SupplierBillingInformation GetDefaultSupplierBilling(SupplierId supplierId)
    {
        return new SupplierBillingInformation(supplierId, "test",  new EmailAddress("test@test.com"), GetDefaultSiret(), new Address("test", null, "70000", "city"));
    }
    
    public static CustomerBillingInformation GetDefaultCustomerBilling(CustomerId customerId)
    {
        return new CustomerBillingInformation(customerId, "test",  new EmailAddress("test@test.com"), GetDefaultSiret(), new Address("test", null, "70000", "city"));
    }
    
    public static Account GetDefaultAccount(PasswordHasher hasher, string email = "test@test.com",
        string password = "P@ssword")
    {
        return new Account(new Username(email), new EmailAddress(email), HashedPassword.Create(password, hasher));
    }

    public static Supplier GetDefaultSupplier(AccountId accountIdentifier, string emailAddress = "test@test.com")
    {
        return new Supplier(new TradeName(accountIdentifier.Value), new EmailAddress($"{accountIdentifier.Value}.{emailAddress}"), new PhoneNumber("0664566565"),
            new Legal(new CorporateName(accountIdentifier.Value), new Siret("15932477173006"), new LegalAddress(accountIdentifier.Value, null, "70000", "Test")), accountIdentifier, new ShippingAddress(accountIdentifier.Value, new EmailAddress("test@est.com"), "70000", "Test", "7000", "city"),
            new BillingAddress(accountIdentifier.Value, new EmailAddress("test@est.com"), "70000", "Test", "7000", "city"));
    }

    public static Customer GetDefaultCustomer(AccountId accountIdentifier, string emailAddress = "test@test.com")
    {
        return new Customer(new TradeName(accountIdentifier.Value), new EmailAddress($"{accountIdentifier.Value}.{emailAddress}"), new PhoneNumber("0664566565"),
            new Legal(new CorporateName(accountIdentifier.Value), new Siret("15932477173006"), new LegalAddress(accountIdentifier.Value, null, "70000", "Test")), accountIdentifier, new DeliveryAddress(accountIdentifier.Value, new EmailAddress("test@est.com"), "70000", "Test", "7000", "city"),
            new BillingAddress(accountIdentifier.Value, new EmailAddress("test@est.com"), "70000", "Test", "7000", "city"));
    }
    
    public static Order CreateOrderWithLines(Supplier supplier, Customer customer, bool isDraft, IEnumerable<Product>? products = null)
    {
        if (!isDraft)
        {
            var order = Order.CreateDraft(supplier.Identifier, customer.Identifier);
            if (products != null && products.Any())
            {
                order.UpdateDraftLines(
                    products.Where(p => p.SupplierIdentifier == supplier.Identifier).Select(p =>
                        OrderLine.CreateProductLine(p.Identifier, p.Reference, p.Name,
                            new OrderedQuantity(1), new ProductUnitPrice(2000), p.Vat)) ?? new List<OrderLine>());

                order.Publish(new OrderReference("test"), order.Lines);
            }

            return order;
        }

        var orderDraft = Order.CreateDraft(supplier.Identifier, customer.Identifier);
        if(products != null && products.Any())
            orderDraft.UpdateDraftLines(
                products.Where(p => p.SupplierIdentifier == supplier.Identifier).Select(p => OrderLine.CreateProductLine(p.Identifier, p.Reference, p.Name,
                    new OrderedQuantity(1), new ProductUnitPrice(2000), p.Vat)) ?? new List<OrderLine>());
        
        return orderDraft;
    }

    public static AppDbContext InitContext(AppDbContext context, 
        List<AccountId> customerIds,
        Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> suppliers,
        Dictionary<AccountId, Dictionary<string, int>> suppliersProducts)
    {
        foreach (var customerId in customerIds)
        {
            var customer = GetDefaultCustomer(customerId);
            context.Add(customer);
        }

        context.SaveChanges();

        foreach (var supplierInfo in suppliers)
        {
            var supplier = GetDefaultSupplier(supplierInfo.Key);
            context.Add(supplier);

            var catalog = Catalog.CreateDefaultCatalog(supplier.Identifier);
            context.Add(catalog);
            
            foreach (var agreementInfo in supplierInfo.Value)
            {
                var customer = context.Customers.Single(s => s.AccountIdentifier == agreementInfo.Key);
            
                var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Identifier, customer.Identifier,
                    catalog.Identifier,
                    new List<DeliveryDay> {agreementInfo.Value}, 2);

                agreement.Accept();
                context.Add(agreement);
            }

            if (!suppliersProducts.TryGetValue(supplierInfo.Key, out var productsInfo)) 
                continue;
            
            foreach (var productInfo in productsInfo)
            {
                var product = new Product(new ProductName(productInfo.Key), new ProductReference(productInfo.Key), new VatRate(2000), null,
                    supplier.Identifier);
                
                catalog.AddOrUpdateProductPrice(product, new ProductUnitPrice(productInfo.Value));
                context.Add(product);
            }
        }

        context.SaveChanges();
        return context;
    }
}