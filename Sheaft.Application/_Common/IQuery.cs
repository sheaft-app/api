using MediatR;
using Sheaft.Domain;

namespace Sheaft.Application;

public interface IQuery<out TResponse> : IRequest<TResponse>{}

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}

public record PagedResult<T>
{
    public PagedResult(IEnumerable<T> items, PageInfo pageInfo, int totalItems)
    {
        Items = items;
        PageInfo = pageInfo;
        TotalItems = totalItems;
        TotalPages = (int) Math.Ceiling(totalItems / (double) pageInfo.Take);
    }
    public IEnumerable<T> Items { get; init; }
    public PageInfo PageInfo { get; init;}
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
}