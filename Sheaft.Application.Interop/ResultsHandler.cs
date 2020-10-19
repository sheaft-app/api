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

        protected async Task<Result<T>> ExecuteAsync<T>(Func<Task<Result<T>>> method)
        {
            try
            {
                var result = await method();
                LogInformation(result);

                return result;
            }
            catch (SheaftException e)
            {
                LogError(e);
                return Failed<T>(e.InnerException ?? e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                LogError(e);
                return Conflict<T>(e.InnerException ?? e);
            }
            catch (DbUpdateException e)
            {
                LogError(e);

                if (e.InnerException != null && e.InnerException.Message.Contains("Cannot insert duplicate key row in object"))
                    return Failed<T>(new AlreadyExistsException(e.InnerException));

                return Failed<T>(e.InnerException ?? e);
            }
            catch (NotSupportedException e)
            {
                LogError(e);
                return Failed<T>(e.InnerException ?? e);
            }
            catch (InvalidOperationException e)
            {
                LogError(e);
                return Failed<T>(e.InnerException ?? e);
            }
            catch (Exception e)
            {
                LogError(e);
                return InternalError<T>(e.InnerException ?? e);
            }
        }

        protected async Task<Result<IEnumerable<T>>> ExecuteAsync<T>(Func<Task<Result<IEnumerable<T>>>> method)
        {
            try
            {
                var result = await method();
                LogInformation(result);

                return result;
            }
            catch (SheaftException e)
            {
                LogError(e);
                return Failed<IEnumerable<T>>(e.InnerException ?? e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                LogError(e);
                return Conflict<IEnumerable<T>>(e.InnerException ?? e);
            }
            catch (DbUpdateException e)
            {
                LogError(e);

                if (e.InnerException != null && e.InnerException.Message.Contains("Cannot insert duplicate key row in object"))
                    return Failed<IEnumerable<T>>(new AlreadyExistsException(e.InnerException));
                                
                return Failed<IEnumerable<T>>(e.InnerException ?? e);
            }
            catch (NotSupportedException e)
            {
                LogError(e);
                return Failed<IEnumerable<T>>(e.InnerException ?? e);
            }
            catch (InvalidOperationException e)
            {
                LogError(e);
                return Failed<IEnumerable<T>>(e.InnerException ?? e);
            }
            catch (Exception e)
            {
                LogError(e);
                return InternalError<IEnumerable<T>>(e.InnerException ?? e);
            }
        }

        private void LogError(Exception e)
        {
            _logger.LogError($"Exception: {{0}}, message: {{1}}", e, e.Message);
        }

        private void LogInformation<T>(Result<T> result)
        {
            _logger.LogTrace($"Succeeded: {result.Success}");
        }

        private void LogInformation<T>(Result<IEnumerable<T>> result)
        {
            _logger.LogTrace($"Succeeded: {result.Success}");
        }
    }
}