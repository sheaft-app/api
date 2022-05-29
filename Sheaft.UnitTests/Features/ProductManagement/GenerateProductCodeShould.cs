using System;
using NUnit.Framework;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.ProductManagement;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.ProductManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class GenerateProductCodeShould
{
    [Test]
    public void Return_Next_Generated_Code_From_Value_In_Database()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<GenerateProductCode>();
        
        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var supplier = DataHelpers.GetDefaultSupplier(supplierAccount.Id);
        context.Add(supplier);
        
        context.Products.Add(new Product(new ProductName("tests"), new ProductReference(1), new VatRate(0), "", supplier.Id));
        context.SaveChanges();
        var generateCode = new GenerateProductCode(context);
        
        var result = generateCode.GenerateNextCode(supplier.Id);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("0000000000024", result.Value.Value);
    }
    
    [Test]
    public void Return_Next_Generated_Code_From_Default_Generated()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<GenerateProductCode>();
        
        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var supplier = DataHelpers.GetDefaultSupplier(supplierAccount.Id);
        context.Add(supplier);
        
        context.Products.Add(new Product(new ProductName("tests"), new ProductReference("PROD1"), new VatRate(0), "",
            supplier.Id));
        
        context.SaveChanges();
        var generateCode = new GenerateProductCode(context);
        
        var result = generateCode.GenerateNextCode(supplier.Id);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("0000000000017", result.Value.Value);
    }
    
    [Test]
    public void Return_Next_Generated_Code_From_Last_Generated_Even_With_Gaps()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<GenerateProductCode>();
        
        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var supplier = DataHelpers.GetDefaultSupplier(supplierAccount.Id);
        context.Add(supplier);
        
        context.Products.Add(new Product(new ProductName("tests 1"), new ProductReference(1), new VatRate(0), "",
            supplier.Id));
        context.Products.Add(new Product(new ProductName("tests 2"), new ProductReference("PROD1"), new VatRate(0), "",
            supplier.Id));
        context.Products.Add(new Product(new ProductName("tests 3"), new ProductReference(20), new VatRate(0), "",
            supplier.Id));
        
        context.SaveChanges();
        var generateCode = new GenerateProductCode(context);
        
        var result = generateCode.GenerateNextCode(supplier.Id);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("0000000000215", result.Value.Value);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618