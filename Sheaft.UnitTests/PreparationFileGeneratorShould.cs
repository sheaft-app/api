using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OfficeOpenXml;
using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;
using Sheaft.Infrastructure.DocumentManagement;

namespace Sheaft.UnitTests;

public class PreparationFileGeneratorShould
{
    [Test]
    public async Task Create_File()
    {
        var generator = new PreparationFileGenerator();
        
        var result = await generator.Generate(_preparationDocumentData, CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
        
        using var stream = new MemoryStream(result.Value);
        using var package = new ExcelPackage(stream);

        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
        Assert.IsNotNull(worksheet);
        
        Assert.AreEqual("Client1", worksheet.Cells["B2"].GetValue<string>());
        Assert.AreEqual("Client2", worksheet.Cells["E2"].GetValue<string>());
        
        Assert.AreEqual("CD202200001", worksheet.Cells["B3"].GetValue<string>());
        Assert.AreEqual("CD202200002", worksheet.Cells["C3"].GetValue<string>());
        Assert.AreEqual("CD202200003", worksheet.Cells["E3"].GetValue<string>());
        
        Assert.AreEqual("PRODUCT 1", worksheet.Cells["A4"].GetValue<string>());
        Assert.AreEqual(5, worksheet.Cells["B4"].GetValue<int>());
        Assert.AreEqual(2, worksheet.Cells["C4"].GetValue<int>());
        Assert.AreEqual(0, worksheet.Cells["E4"].GetValue<int>());
        
        Assert.AreEqual("PRODUCT 2", worksheet.Cells["A5"].GetValue<string>());
        Assert.AreEqual(0, worksheet.Cells["B5"].GetValue<int>());
        Assert.AreEqual(4, worksheet.Cells["C5"].GetValue<int>());
        Assert.AreEqual(1, worksheet.Cells["E5"].GetValue<int>());
    }
    
    private PreparationDocumentData _preparationDocumentData = new(new List<ProductToPrepare>
    {
        new ProductToPrepare(new ProductReference(1), new ProductName("PRODUCT 1"),
            new List<QuantityPerOrder>
            {
                new QuantityPerOrder(new OrderReference(1), new Quantity(5)),
                new QuantityPerOrder(new OrderReference(2), new Quantity(2))
            }),
        new ProductToPrepare(new ProductReference(2), new ProductName("PRODUCT 2"),
            new List<QuantityPerOrder>
            {
                new QuantityPerOrder(new OrderReference(2), new Quantity(4)),
                new QuantityPerOrder(new OrderReference(3), new Quantity(1))
            })
    },
    new List<ClientOrdersToPrepare>
    {
        new ClientOrdersToPrepare("Client1", new List<OrderReference>
        {
            new OrderReference(1),
            new OrderReference(2),
        }),
        new ClientOrdersToPrepare("Client2", new List<OrderReference>
        {
            new OrderReference(3)
        })
    });
}