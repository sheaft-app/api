using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public interface IOrderQueries
{
    Task<Result<OrderDetailsDto>> Get(OrderId identifier, AccountId accountId, CancellationToken token);
    Task<Result<PagedResult<OrderDto>>> List(AccountId accountId, PageInfo pageInfo, CancellationToken token);
}