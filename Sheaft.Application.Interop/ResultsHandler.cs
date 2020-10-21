using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Sheaft.Exceptions;

namespace Sheaft.Application.Interop
{
    public class ResultsHandler
    {
        protected readonly ILogger _logger;
        protected readonly ISheaftMediatr _mediatr;
        protected readonly IAppDbContext _context;

        public ResultsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger logger)
        {
            _mediatr = mediatr;
            _context = context;
            _logger = logger;
        }

        protected Result<T> Ok<T>(T result, MessageKind? message = null, params object[] objs)
        {
            return new SuccessResult<T>(result, message, objs);
        }
        protected Result<T> ValidationError<T>(MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new ValidationException(message, objs));
        }
        protected Result<T> BadRequest<T>(MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new BadRequestException(message, objs));
        }
        protected Result<T> Conflict<T>(MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new ConflictException(message, objs));
        }
        protected Result<T> Conflict<T>(Exception exception, MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new ConflictException(exception, message, objs));
        }

        protected Result<T> Unauthorized<T>(MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new UnauthorizedException(message, objs));
        }

        protected Result<T> Forbidden<T>(MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new ForbiddenException(message, objs));
        }

        protected Result<T> NotFound<T>(MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new NotFoundException(message, objs));
        }

        protected Result<T> Created<T>(T result, MessageKind? message = null, params object[] objs)
        {
            return new SuccessResult<T>(result, message, objs);
        }

        protected Result<T> Accepted<T>(T result, MessageKind? message = null, params object[] objs)
        {
            return new SuccessResult<T>(result, message);
        }

        protected Result<T> Locked<T>(MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new LockedException(message, objs));
        }

        protected Result<T> TooManyRetries<T>(MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new TooManyRetriesException(message, objs));
        }

        protected Result<T> InternalError<T>(MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new UnexpectedException(message, objs));
        }

        protected Result<T> InternalError<T>(Exception exception, MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new UnexpectedException(exception, message, objs));
        }

        protected Result<T> Failed<T>(Exception exception, MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(exception, message, objs);
        }

        protected Result<T> Failed<T>(SheaftException exception, MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(exception, message, objs);
        }

        // IEnumerable

        protected Result<IEnumerable<T>> Ok<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected Result<IEnumerable<T>> Created<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected Result<IEnumerable<T>> Accepted<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected Result<T> HandleException<T>(ICommand<T> request, Exception e)
        {
            var type = request.GetType().Name;

            if (e is SheaftException sheaftException)
            {
                _logger.LogError(sheaftException, $"Error on executing {type} : {sheaftException.Message}");
                return Failed<T>(sheaftException.InnerException ?? sheaftException);
            }

            if (e is DbUpdateConcurrencyException dbUpdateConcurrency)
            {
                _logger.LogError(dbUpdateConcurrency, $"Error on executing {type} : {dbUpdateConcurrency.Message}");
                return Conflict<T>(dbUpdateConcurrency.InnerException ?? dbUpdateConcurrency);
            }

            if (e is DbUpdateException dbUpdate)
            {
                _logger.LogError(dbUpdate, $"Error on executing {type} : {dbUpdate.Message}");

                if (dbUpdate.InnerException != null && dbUpdate.InnerException.Message.Contains("Cannot insert duplicate key row in object"))
                    return Failed<T>(new AlreadyExistsException(dbUpdate.InnerException));

                return Failed<T>(dbUpdate.InnerException ?? dbUpdate);
            }

            if (e is NotSupportedException notSupported)
            {
                _logger.LogError(notSupported, $"Error on executing {type} : {notSupported.Message}");
                return Failed<T>(notSupported.InnerException ?? notSupported);
            }

            if (e is InvalidOperationException invalidOperation)
            {
                _logger.LogError(invalidOperation, $"Error on executing {type} : {invalidOperation.Message}");
                return Failed<T>(invalidOperation.InnerException ?? invalidOperation);
            }

            _logger.LogError(e, $"Error on executing {type} : {e.Message}");
            return InternalError<T>(e.InnerException ?? e);
        }

        protected async Task<Result<T>> ExecuteAsync<T>(ICommand<T> request, Func<Task<Result<T>>> method)
        {
            var type = request.GetType().Name;
            using (var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = request.RequestUser.RequestId,
                ["UserIdentifier"] = request.RequestUser.Id.ToString("N"),
                ["Roles"] = string.Join(';', request.RequestUser.Roles),
                ["IsAuthenticated"] = request.RequestUser.IsAuthenticated,
                ["Command"] = type,
            }))
            {
                try
                {
                    _logger.LogInformation($"Executing {type}");
                    var result = await method();

                    if (result.Success)
                        _logger.LogDebug($"{type} succeeded.");
                    else
                        _logger.LogDebug($"{type} failed.");

                    return result;
                }
                catch (Exception e)
                {
                    return HandleException(request, e);
                }
            }
        }

        protected async Task<Result<IEnumerable<T>>> ExecuteAsync<T>(ICommand<IEnumerable<T>> request, Func<Task<Result<IEnumerable<T>>>> method)
        {
            var type = request.GetType().Name;
            using (var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = request.RequestUser.RequestId,
                ["UserIdentifier"] = request.RequestUser.Id.ToString("N"),
                ["Roles"] = string.Join(';', request.RequestUser.Roles),
                ["IsAuthenticated"] = request.RequestUser.IsAuthenticated,
                ["Command"] = type,
            }))
            {
                try
                {
                    _logger.LogInformation($"Executing {type}");
                    var result = await method();

                    if (result.Success)
                        _logger.LogDebug($"{type} succeeded.");
                    else
                        _logger.LogDebug($"{type} failed.");

                    return result;
                }
                catch (Exception e)
                {
                    return HandleException(request, e);
                }
            }
        }
    }
}