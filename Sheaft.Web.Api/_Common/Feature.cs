using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Domain;
using Sheaft.Infrastructure;
#pragma warning disable CS8604

namespace Sheaft.Web.Api;

[Authorize]
[ApiController]
public class Feature : ControllerBase
{
    protected readonly ISheaftMediator Mediator;
    protected AccountId? CurrentAccountId => HttpContext.User.Identity?.IsAuthenticated == true 
        ? new AccountId(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) 
        : null;

    public Feature(ISheaftMediator mediator)
    {
        Mediator = mediator;
    }

    protected ActionResult<T> HandleQueryResult<T>(Result<T> result)
    {
        return result.IsSuccess
            ? new ObjectResult(result.Value){StatusCode = StatusCodes.Status200OK}
            : HandleErrorResult(result);
    }

    protected ActionResult<PaginatedResults<T>> HandleQueryResult<T>(Result<PagedResult<T>> result)
    {
        return result.IsSuccess
            ? GetPaginatedResults(result.Value)
            : HandleErrorResult(result);
    }

    protected ActionResult<T> HandleCommandResult<T>(Result<T> result)
    {
        return result.IsSuccess 
            ? new ObjectResult(result.Value){StatusCode = StatusCodes.Status200OK} 
            : HandleErrorResult(result);
    }

    protected ActionResult<U> HandleCommandResult<T, U>(Result<T> result)
    {
        return result.IsSuccess 
            ? new ObjectResult(result.Value.Adapt<U>()){StatusCode = StatusCodes.Status200OK} 
            : HandleErrorResult(result);
    }

    protected ActionResult HandleCommandResult(Result result)
    {
        return result.IsSuccess 
            ? new EmptyResult() 
            : HandleErrorResult(result);
    }

    private ActionResult<PaginatedResults<T>> GetPaginatedResults<T>(PagedResult<T> orders)
    {
        return new ObjectResult(new PaginatedResults<T>(
            orders.Items,
            orders.PageInfo.Skip < orders.TotalItems
                ? $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}?page={orders.PageInfo.Page + 1}&take={orders.PageInfo.Take}"
                : null,
            orders.PageInfo.Page > 1
                ? $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}?page={orders.PageInfo.Page - 1}&take={orders.PageInfo.Take}"
                : null,
            orders.PageInfo.Page,
            orders.PageInfo.Take,
            orders.TotalItems,
            orders.TotalPages)){StatusCode = StatusCodes.Status200OK};
    }

    private ActionResult HandleErrorResult(Result result)
    {
        var responseContent = ProblemDetailsFactory.CreateProblemDetails(
            HttpContext, GetStatusCode(result), result.Error.Code, detail: result.Error.Message,
            instance: HttpContext.Request.Path);

        responseContent.Type = $"https://httpstatuses.com/{responseContent.Status}";
        
        if (result.Error.Extensions != null)
            foreach (var (key, value) in result.Error.Extensions)
                responseContent.Extensions.Add(key, value);

        return new ObjectResult(responseContent) {StatusCode = responseContent.Status};
    }

    private int GetStatusCode(Result result)
    {
        switch (result.Error.Kind)
        {
            case ErrorKind.NotFound:
                return StatusCodes.Status404NotFound;
            case ErrorKind.Conflict:
                return StatusCodes.Status409Conflict;
            case ErrorKind.Forbidden:
                return StatusCodes.Status403Forbidden;
            case ErrorKind.Unauthorized:
                return StatusCodes.Status401Unauthorized;
            case ErrorKind.Validation:
                return StatusCodes.Status422UnprocessableEntity;
            case ErrorKind.BadRequest:
            case ErrorKind.Unexpected:
            default:
                return StatusCodes.Status400BadRequest;
        }
    }
}

public record PaginatedResults<T>
{
    public PaginatedResults(IEnumerable<T> items, string? next, string? previous, int pageNumber, int itemsPerPage, int totalItems, int totalPages)
    {
        Items = items;
        Next = next;
        Previous = previous;
        PageNumber = pageNumber;
        ItemsPerPage = itemsPerPage;
        TotalItems = totalItems;
        TotalPages = totalPages;
    }

    public IEnumerable<T> Items { get; private set; }
    public string? Next { get; private set; }
    public string? Previous { get; private set; }
    public int PageNumber { get; private set; }
    public int ItemsPerPage { get; private set; }
    public int TotalItems { get; private set; }
    public int TotalPages { get; private set; }
}