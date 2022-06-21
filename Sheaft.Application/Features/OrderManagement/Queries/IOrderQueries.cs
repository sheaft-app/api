using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public interface IOrderQueries
{
    Task<Result<OrderDraftDto>> GetDraft(OrderId identifier, AccountId accountId, CancellationToken token);
    Task<Result<OrderDetailsDto>> Get(OrderId identifier, AccountId accountId, CancellationToken token);
    Task<Result<PagedResult<OrderDto>>> List(AccountId accountId, IEnumerable<OrderStatus> statuses, PageInfo pageInfo, CancellationToken token);
}