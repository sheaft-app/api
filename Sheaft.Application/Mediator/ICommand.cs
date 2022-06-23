using MediatR;
using Sheaft.Domain;

namespace Sheaft.Application;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    DateTimeOffset CreatedAt { get; }
    RequestUser RequestUser { get; }
    void SetRequestUser(RequestUser user);
}

public record Command<TResponse> : ICommand<TResponse>
{
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
    public RequestUser RequestUser { get; private set; }

    public void SetRequestUser(RequestUser user)
    {
        RequestUser = user;
    }
}