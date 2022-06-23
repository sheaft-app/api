using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application;
using Sheaft.Domain;

namespace Sheaft.Infrastructure;

internal class SheaftMediator : ISheaftMediator
{
    private readonly IMediator _mediatr;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ILogger<SheaftMediator> _logger;

    public SheaftMediator(
        IMediator mediatr,
        ICurrentUserService currentUserService,
        IBackgroundJobClient backgroundJobClient,
        ILogger<SheaftMediator> logger)
    {
        _mediatr = mediatr;
        _currentUserService = currentUserService;
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
        
        var requestUserResult = _currentUserService.GetCurrentUserInfo();
        if (requestUserResult.IsFailure)
            throw new InvalidOperationException(requestUserResult.Error.Message);
        
        command.SetRequestUser(requestUserResult.Value);
        _backgroundJobClient.Enqueue<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
    }

    public void Post<T>(ICommand<Result<T>> command, string? jobName = null)
    {
        var name = jobName ?? command.GetType().Name;
        _logger.LogTrace("Posting command<T> {Name}", name);
        
        var requestUserResult = _currentUserService.GetCurrentUserInfo();
        if (requestUserResult.IsFailure)
            throw new InvalidOperationException(requestUserResult.Error.Message);
        
        command.SetRequestUser(requestUserResult.Value);
        _backgroundJobClient.Enqueue<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
    }

    public void Schedule(ICommand<Result> command, TimeSpan delay, string? jobName = null)
    {
        var name = jobName ?? command.GetType().Name;
        _logger.LogTrace("Scheduling command {Name}", name);
        
        var requestUserResult = _currentUserService.GetCurrentUserInfo();
        if (requestUserResult.IsFailure)
            throw new InvalidOperationException(requestUserResult.Error.Message);
        
        command.SetRequestUser(requestUserResult.Value);
        _backgroundJobClient.Schedule<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None),
            delay);
    }

    public void Schedule<T>(ICommand<Result<T>> command, TimeSpan delay, string? jobName = null)
    {
        var name = jobName ?? command.GetType().Name;
        _logger.LogTrace("Scheduling command<T> {Name}", name);
        
        var requestUserResult = _currentUserService.GetCurrentUserInfo();
        if (requestUserResult.IsFailure)
            throw new InvalidOperationException(requestUserResult.Error.Message);
        
        command.SetRequestUser(requestUserResult.Value);
        _backgroundJobClient.Schedule<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None),
            delay);
    }

    public async Task<Result> Execute(ICommand<Result> command, CancellationToken token)
    {
        var requestUserResult = _currentUserService.GetCurrentUserInfo();
        if (requestUserResult.IsFailure)
            throw new InvalidOperationException(requestUserResult.Error.Message);
        
        command.SetRequestUser(requestUserResult.Value);
        return await _mediatr.Send(command, token);
    }

    public async Task<Result<T>> Execute<T>(ICommand<Result<T>> command, CancellationToken token)
    {
        var requestUserResult = _currentUserService.GetCurrentUserInfo();
        if (requestUserResult.IsFailure)
            throw new InvalidOperationException(requestUserResult.Error.Message);
        
        command.SetRequestUser(requestUserResult.Value);
        return await _mediatr.Send(command, token);
    }

    public async Task<Result> Query(IQuery<Result> query, CancellationToken token)
    {
        var requestUserResult = _currentUserService.GetCurrentUserInfo();
        if (requestUserResult.IsFailure)
            throw new InvalidOperationException(requestUserResult.Error.Message);
        
        query.SetRequestUser(requestUserResult.Value);
        return await _mediatr.Send(query, token);
    }

    public async Task<Result<T>> Query<T>(IQuery<Result<T>> query, CancellationToken token)
    {
        var requestUserResult = _currentUserService.GetCurrentUserInfo();
        if (requestUserResult.IsFailure)
            throw new InvalidOperationException(requestUserResult.Error.Message);
        
        query.SetRequestUser(requestUserResult.Value);
        return await _mediatr.Send(query, token);
    }
}