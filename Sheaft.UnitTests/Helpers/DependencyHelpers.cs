using Sheaft.Application;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Fakes;

namespace Sheaft.UnitTests.Helpers;

internal static class DependencyHelpers
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