using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.AgreementManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.AgreementManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class ProposeAgreementToSupplierCommandShould
{
    [Test]
    public async Task Create_Agreement_Between_Retailer_And_Supplier()
    {
        var (context, handler) = InitHandler();
        var command = GetCommand();

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
    }

    private (AppDbContext, ProposeAgreementToSupplierHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ProposeAgreementToSupplierHandler>();
        var handler = new ProposeAgreementToSupplierHandler(uow);
        
        return (context, handler);
    }

    private static ProposeAgreementToSupplierCommand GetCommand()
    {
        var command = new ProposeAgreementToSupplierCommand();
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
