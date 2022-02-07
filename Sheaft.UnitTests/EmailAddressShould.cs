using NUnit.Framework;
using Sheaft.Domain;

namespace Sheaft.UnitTests;

public class EmailAddressShould
{
    [Test]
    public void Throw_If_Null_Or_Empty()
    {
        Assert.That(() => new EmailAddress(""), Throws.ArgumentNullException);
    }

    [Test]
    public void Throw_If_Invalid_Pattern()
    {
        Assert.That(() => new EmailAddress("email,mug@est."), Throws.InvalidOperationException);
    }

    [Test]
    public void Return_Valid_Email()
    {
        var emailAddress = new EmailAddress("email.mug+test@est.fr");
        
        Assert.IsNotNull(emailAddress);
        Assert.IsNotEmpty(emailAddress.Email);
    }
}