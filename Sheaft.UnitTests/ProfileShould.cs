using NUnit.Framework;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.AccountManagement.ValueObjects;
using Sheaft.Domain.Address;

namespace Sheaft.UnitTests;

public class ProfileShould
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Initialize_Profile_Data()
    {
        var manufacturer = InitProfile();

        Assert.IsNotNull(manufacturer);
        Assert.IsNotEmpty(manufacturer.Name.Value);
    }

    private Profile InitProfile()
    {
        return new Profile(
            new CompanyName("name"),
            new EmailAddress("test@test.com"),
            new PhoneNumber("0601020101"),
            new Legal(
                new LegalName("name"),
                new Siret("15932477173006"),
                new Address("line1", null, "73410", "city")
            ));
    }
}