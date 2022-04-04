using MediatR;

namespace Sheaft.Application;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

public record Command<TResponse> : ICommand<TResponse>
{
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
}