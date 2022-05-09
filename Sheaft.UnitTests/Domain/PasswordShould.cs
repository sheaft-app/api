using NUnit.Framework;
using Sheaft.Domain;

namespace Sheaft.UnitTests.Domain;

public class PasswordShould
{
    [Test]
    public void Throw_If_Null_Or_Empty()
    {
        Assert.That(() => new Password(""), Throws.InvalidOperationException);
    }

    [Test]
    public void Throw_If_Too_Short()
    {
        Assert.That(() => new Password("aze5"), Throws.InvalidOperationException);
    }

    [Test]
    public void Return_Valid_Password()
    {
        var password = new Password("aze5654454");
        
        Assert.IsNotNull(password);
        Assert.IsNotEmpty(password.Value);
    }
}