using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application;
using Sheaft.Domain;

namespace Sheaft.Infrastructure;

internal class SheaftMediator : ISheaftMediator
{
    private readonly IMediator _mediatr;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ILogger<SheaftMediator> _logger;

    public SheaftMediator(
        IMediator mediatr,
        IBackgroundJobClient backgroundJobClient,
        ILogger<SheaftMediator> logger)
    {
        _mediatr = mediatr;
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
    }

    public void Publish(IDomainEvent notification, string? jobName = null)
    {
        var name = jobName ?? notification.GetType().Name;
        _logger.LogTrace("Publishing integration event {Name}", name);
        _backgroundJobClient.Enqueue<SheaftDispatcher>(bridge =>
            bridge.Execute(name, notification, CancellationToken.None));
    }

    public void Post(ICommand<Result> command, string? jobName = null)
    {
        var name = jobName ?? command.GetType().Name;
        _logger.LogTrace("Posting command {Name}", name);
        _backgroundJobClient.Enqueue<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
    }

    public void Post<T>(ICommand<Result<T>> command, string? jobName = null)
    {
        var name = jobName ?? command.GetType().Name;
        _logger.LogTrace("Posting command<T> {Name}", name);
        _backgroundJobClient.Enqueue<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
    }

    public void Schedule(ICommand<Result> command, TimeSpan delay, string? jobName = null)
    {
        var name = jobName ?? command.GetType().Name;
        _logger.LogTrace("Scheduling command {Name}", name);
        _backgroundJobClient.Schedule<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None),
            delay);
    }

    public void Schedule<T>(ICommand<Result<T>> command, TimeSpan delay, string? jobName = null)
    {
        var name = jobName ?? command.GetType().Name;
        _logger.LogTrace("Scheduling command<T> {Name}", name);
        _backgroundJobClient.Schedule<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None),
            delay);
    }

    public async Task<Result> Execute(ICommand<Result> command, CancellationToken token)
    {
        return await _mediatr.Send(command, token);
    }

    public async Task<Result<T>> Execute<T>(ICommand<Result<T>> command, CancellationToken token)
    {
        return await _mediatr.Send(command, token);
    }

    public async Task<Result> Query(IQuery<Result> command, CancellationToken token)
    {
        return await _mediatr.Send(command, token);
    }

    public async Task<Result<T>> Query<T>(IQuery<Result<T>> command, CancellationToken token)
    {
        return await _mediatr.Send(command, token);
    }
}