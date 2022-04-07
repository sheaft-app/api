﻿using System;
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
    public static Account GetDefaultAccount(PasswordHasher hasher, string email = "test@test.com",
        string password = "P@ssword")
    {
        return new Account(new Username(email), new EmailAddress(email), HashedPassword.Create(password, hasher));
    }

    public static Supplier GetDefaultSupplier(AccountId accountIdentifier, string emailAddress = "test@test.com")
    {
        return new Supplier(new TradeName(accountIdentifier.Value), new EmailAddress($"{accountIdentifier.Value}.{emailAddress}"), new PhoneNumber("0664566565"),
            new Legal(new CorporateName(accountIdentifier.Value), new Siret("15932477173006"), new LegalAddress(accountIdentifier.Value, null, "70000", "Test")), new ShippingAddress(accountIdentifier.Value, null, "70000", "Test"),
            accountIdentifier);
    }

    public static Customer GetDefaultCustomer(AccountId accountIdentifier, string emailAddress = "test@test.com")
    {
        return new Customer(new TradeName(accountIdentifier.Value), new EmailAddress($"{accountIdentifier.Value}.{emailAddress}"), new PhoneNumber("0664566565"),
            new Legal(new CorporateName(accountIdentifier.Value), new Siret("15932477173006"), new LegalAddress(accountIdentifier.Value, null, "70000", "Test")), new DeliveryAddress(accountIdentifier.Value, null, "70000", "Test"),
            accountIdentifier);
    }
    
    public static Order CreateOrderWithLines(Supplier supplier, Customer customer, bool isDraft, bool addProducts = true)
    {
        if (!isDraft)
        {
            var order = Order.Create(new OrderReference(Guid.NewGuid().ToString("N")), supplier.Identifier, customer.Identifier,
                addProducts ? new List<OrderLine>
                {
                    OrderLine.CreateProductLine(new ProductId("test1"), new ProductReference("test1"), new ProductName("test1"),
                        new OrderedQuantity(1),
                        new ProductUnitPrice(2000), new VatRate(2000)),
                    OrderLine.CreateProductLine(new ProductId("test2"), new ProductReference("test2"), new ProductName("test2"),
                        new OrderedQuantity(1),
                        new ProductUnitPrice(2000), new VatRate(2000))
                } : new List<OrderLine>(), "externalCode");
            return order;
        }

        var orderDraft = Order.CreateDraft(supplier.Identifier, customer.Identifier);
        if(addProducts)
            orderDraft.UpdateDraftLines(
                new List<OrderLine>
                    {
                        OrderLine.CreateProductLine(new ProductId("test1"), new ProductReference("test1"), new ProductName("test1"),
                            new OrderedQuantity(1),
                            new ProductUnitPrice(2000), new VatRate(2000)),
                        OrderLine.CreateProductLine(new ProductId("test2"), new ProductReference("test2"), new ProductName("test2"),
                            new OrderedQuantity(1),
                            new ProductUnitPrice(2000), new VatRate(2000))
                    });
        
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
            
                var agreement = Agreement.CreateSupplierAgreement(supplier.Identifier, customer.Identifier,
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