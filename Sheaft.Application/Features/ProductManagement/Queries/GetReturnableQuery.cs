using Sheaft.Domain;

namespace Sheaft.Application.ProductManagement;

public record GetReturnableQuery(ReturnableId Identifier, SupplierId SupplierId) : Query<Result<ReturnableDto>>;

internal class GetReturnableHandler : IQueryHandler<GetReturnableQuery, Result<ReturnableDto>>
{
    private readonly IReturnableQueries _returnableQueries;

    public GetReturnableHandler(IReturnableQueries returnableQueries)
    {
        _returnableQueries = returnableQueries;
    }
    
    public async Task<Result<ReturnableDto>> Handle(GetReturnableQuery request, CancellationToken token)
    {
        return await _returnableQueries.Get(request.Identifier, token);
    }
}