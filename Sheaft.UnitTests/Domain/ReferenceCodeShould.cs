using System;
using NUnit.Framework;
using Sheaft.Domain;

namespace Sheaft.UnitTests.Domain;

public class ReferenceCodeShould
{
    [Test]
    public void Return_Valid_OrderReference()
    {
        var currentDate = DateTimeOffset.UtcNow;
        var reference = new OrderReference(1, currentDate);

        Assert.AreEqual($"CD{currentDate.Year}-00001", reference.Value);
    }
    
    [Test]
    public void Return_Valid_InvoiceReference()
    {
        var currentDate = DateTimeOffset.UtcNow;
        var reference = new InvoiceReference(1, currentDate);

        Assert.AreEqual($"FCT{currentDate.Year}-00001", reference.Value);
    }
    
    [Test]
    public void Return_Valid_CreditNoteReference()
    {
        var currentDate = DateTimeOffset.UtcNow;
        var reference = new CreditNoteReference(1, currentDate);

        Assert.AreEqual($"AVR{currentDate.Year}-00001", reference.Value);
    }
    
    [Test]
    public void Return_Valid_ProductReference()
    {
        var reference = new ProductReference(1);

        Assert.AreEqual("0000000000017", reference.Value);
    }
    
    [Test]
    public void Return_Valid_DeliveryReference()
    {
        var currentDate = DateTimeOffset.UtcNow;
        var reference = new DeliveryReference(1, currentDate);

        Assert.AreEqual($"LIV{currentDate.Year}-00001", reference.Value);
    }
    
    [Test]
    public void Return_Valid_DeliveryNoteReference()
    {
        var currentDate = DateTimeOffset.UtcNow;
        var reference = new DeliveryReference(1, currentDate).GetDeliveryNoteReference();

        Assert.AreEqual($"BL{currentDate.Year}-00001", reference.Value);
    }
    
    [Test]
    public void Return_Valid_DeliveryReceiptReference()
    {
        var currentDate = DateTimeOffset.UtcNow;
        var reference = new DeliveryReference(1, currentDate).GetDeliveryReceiptReference();

        Assert.AreEqual($"BR{currentDate.Year}-00001", reference.Value);
    }
}