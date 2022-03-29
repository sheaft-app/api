using NUnit.Framework;
using Sheaft.Domain;

namespace Sheaft.UnitTests;

public class PhoneShould
{
    [Test]
    public void Throw_If_Null_Or_Empty()
    {
        Assert.That(() => new PhoneNumber(""), Throws.InvalidOperationException);
    }

    [Test]
    public void Throw_If_Invalid_Pattern()
    {
        Assert.That(() => new PhoneNumber("062154"), Throws.InvalidOperationException);
    }

    [Test]
    public void Return_Valid_Email()
    {
        var phoneNumber = new PhoneNumber("0632125456");
        
        Assert.IsNotNull(phoneNumber);
        Assert.IsNotEmpty(phoneNumber.Value);
    }
}