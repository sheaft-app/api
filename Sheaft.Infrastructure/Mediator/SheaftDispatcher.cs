using System.ComponentModel;
using MediatR;
using Sheaft.Application;
using Sheaft.Domain;
#pragma warning disable CS8600
#pragma warning disable CS8603

namespace Sheaft.Infrastructure;

internal class SheaftDispatcher : ISheaftDispatcher
{
    private readonly IMediator _mediatr;

    public SheaftDispatcher(IMediator mediator)
    {
        _mediatr = mediator;
    }

    [DisplayName("{0}")]
    public async Task<Result> Execute(string jobName, ICommand<Result> data, CancellationToken token)
    {
        return await _mediatr.Send(data, token);
    }

    [DisplayName("{0}")]
    public async Task<Result<T>> Execute<T>(string jobName, ICommand<Result<T>> data, CancellationToken token)
    {
        return await _mediatr.Send(data, token);
    }

    [DisplayName("{0}")]
    public async Task Execute(string jobName, IDomainEvent data, CancellationToken token)
    {
        await _mediatr.Publish(GetNotificationCorrespondingToDomainEvent(data), token);
    }

    private INotification GetNotificationCorrespondingToDomainEvent(IDomainEvent domainEvent)
    {
        return (INotification) Activator.CreateInstance(
            typeof(WrappedEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
    }
}