using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.BatchManagement;
using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.BatchManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class CreateBatchCommandShould
{
    [Test]
    public async Task Insert_Batch()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);

        var batch = context.Batches.Single(s => s.Identifier == new BatchId(result.Value));
        Assert.IsNotNull(batch);
        Assert.AreEqual("test", batch.Number.Value);
        Assert.AreEqual(BatchDateKind.DDC, batch.DateKind);
    }

    [Test]
    public async Task Fail_If_Number_Already_Exists()
    {
        var (supplierId, context, handler) = InitHandler();
        var command = GetCommand(supplierId, "0001");

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.Conflict, result.Error.Kind);
        Assert.AreEqual("batch.number.already.exists", result.Error.Code);
    }

    private (SupplierId, AppDbContext, CreateBatchHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateBatchHandler>();

        var handler = new CreateBatchHandler(uow);

        var supplierId = SupplierId.New();

        context.Add(new Batch(new BatchNumber("0001"), BatchDateKind.DDC, DateOnly.FromDateTime(DateTime.UtcNow), supplierId));
        context.SaveChanges();
        
        return (supplierId, context, handler);
    }

    private CreateBatchCommand GetCommand(SupplierId supplierIdentifier, string? number = null)
    {
        var command = new CreateBatchCommand(number ?? "test", BatchDateKind.DDC, DateOnly.FromDateTime(DateTime.UtcNow), supplierIdentifier);
        return command;
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618