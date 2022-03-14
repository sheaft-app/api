using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Sheaft.IntegrationTests.Fakes;

#pragma warning disable CS8767
#pragma warning disable CS8618
#pragma warning disable CS8603
public class FakeMediator : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
    {
        return default;
    }

    public Task<object?> Send(object request, CancellationToken cancellationToken = new CancellationToken())
    {
        return default;
    }

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return default;
    }

    public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = new CancellationToken())
    {
        return default;
    }

    public Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.CompletedTask;
    }

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification
    {
        return Task.CompletedTask;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
#pragma warning restore CS8603