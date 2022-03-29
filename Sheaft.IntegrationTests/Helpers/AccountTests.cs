using Sheaft.Application;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Fakes;

namespace Sheaft.IntegrationTests.Helpers;

public static class AccountTests
{
    public static Account GetDefaultAccount(PasswordHasher hasher, string email = "test@test.com", string password = "P@ssword")
    {
        return new Account(new Username(email), new EmailAddress(email), HashedPassword.Create(password, hasher));
    }
}

public static class DependencyHelpers
{
    public static AppDbContext GetDefaultContext()
    {
        var context = new FakeDbContextFactory().CreateContext();
        return context;
    }
    
    public static IUnitOfWork GetDefaultUow(AppDbContext context)
    {
        return new UnitOfWork(new FakeMediator(), context, new FakeLogger<UnitOfWork>());
    }

    public static (AppDbContext, IUnitOfWork, FakeLogger<T>) InitDependencies<T>()
    {
        var context = GetDefaultContext();
        return (context, GetDefaultUow(context), new FakeLogger<T>());
    }
}