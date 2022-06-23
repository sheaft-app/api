using MediatR;
using Sheaft.Domain;

namespace Sheaft.Application;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
    DateTimeOffset CreatedAt { get; }
    RequestUser RequestUser { get; }
    void SetRequestUser(RequestUser user);
}

public record Query<TResponse> : IQuery<TResponse>
{
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
    public RequestUser RequestUser { get; private set; }

    public void SetRequestUser(RequestUser user)
    {
        RequestUser = user;
    }
}

public record PagedResult<T>
{
    public PagedResult(IEnumerable<T>? items, PageInfo pageInfo, int totalItems)
    {
        Items = items ?? new List<T>();
        PageInfo = pageInfo;
        TotalItems = totalItems;
        TotalPages = (int) Math.Ceiling(totalItems / (double) pageInfo.Take);
    }
    public IEnumerable<T> Items { get; init; }
    public PageInfo PageInfo { get; init;}
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
}