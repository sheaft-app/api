using NUnit.Framework;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.UnitTests;

public class SiretShould
{
    [Test]
    public void Throw_If_Null_Or_Empty()
    {
        Assert.That(() => new Siret(""), Throws.ArgumentNullException);
    }

    [Test]
    public void Throw_If_Invalid_Pattern()
    {
        Assert.That(() => new Siret("15932FF73006"), Throws.InvalidOperationException);
    }

    [Test]
    public void Return_Valid_Siret()
    {
        var siret = new Siret("15932477173006");
        
        Assert.IsNotNull(siret);
        Assert.IsNotEmpty(siret.Value);
    }
    
    [Test]
    public void Return_Valid_Siren()
    {
        var siren = new Siret("15932477173006").Siren;
        
        Assert.IsNotNull(siren.Value);
        Assert.AreEqual(9, siren.Value.Length);
        Assert.AreEqual("159324771", siren.Value);
    }
    
    [Test]
    public void Return_Valid_VatNumber()
    {
        var vatNumber = new Siret("15932477173006").VatNumber;
        
        Assert.IsNotNull(vatNumber.Value);
        Assert.AreEqual(13, vatNumber.Value.Length);
        Assert.AreEqual(35, vatNumber.Key);
        Assert.AreEqual("FR35159324771", vatNumber.Value);
    }
    
    [Test]
    public void Return_Valid_NIC()
    {
        var nic = new Siret("15932477173006").NIC;
        
        Assert.IsNotNull(nic.Value);
        Assert.AreEqual(5, nic.Value.Length);
        Assert.AreEqual("73006", nic.Value);
    }
}