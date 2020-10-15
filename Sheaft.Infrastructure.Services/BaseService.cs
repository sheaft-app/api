using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Sheaft.Exceptions;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sheaft.Infrastructure.Services
{
    public class BaseService
    {
        protected readonly ILogger _logger;

        public BaseService(ILogger logger)
        {
            _logger = logger;
        }

        protected Result<T> Ok<T>(T result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Ok), result, message, objs);
            return new SuccessResult<T>(result, message, objs);
        }
        protected Result<T> ValidationError<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.ValidationError), message, objs);
            return new FailedResult<T>(new ValidationException(message, objs));
        }
        protected Result<T> BadRequest<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.BadRequest), message, objs);
            return new FailedResult<T>(new BadRequestException(message, objs));
        }
        protected Result<T> Conflict<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Conflict), message, objs);
            return new FailedResult<T>(new ConflictException(message, objs));
        }

        protected Result<T> Unauthorized<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Unauthorized), message, objs);
            return new FailedResult<T>(new UnauthorizedException(message, objs));
        }

        protected Result<T> Forbidden<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Forbidden), message, objs);
            return new FailedResult<T>(new ForbiddenException(message, objs));
        }

        protected Result<T> NotFound<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.NotFound), message, objs);
            return new FailedResult<T>(new NotFoundException(message, objs));
        }

        protected Result<T> Created<T>(T result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Created), result, message, objs);
            return new SuccessResult<T>(result, message, objs);
        }

        protected Result<T> Accepted<T>(T result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Accepted), result, message, objs);
            return new SuccessResult<T>(result, message);
        }

        protected Result<T> Locked<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Locked), message, objs);
            return new FailedResult<T>(new LockedException(message, objs));
        }

        protected Result<T> TooManyRetries<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Locked), message, objs);
            return new FailedResult<T>(new TooManyRetriesException(message, objs));
        }

        protected Result<T> InternalError<T>(MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.InternalError), message, objs);
            return new FailedResult<T>(new UnexpectedException(message, objs));
        }

        protected Result<T> Failed<T>(Exception exception, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Failed), exception, message, objs);
            return new FailedResult<T>(exception, message, objs);
        }

        protected Result<T> Failed<T>(SheaftException exception, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Failed), exception, message, objs);
            return new FailedResult<T>(exception, message, objs);
        }

        // IEnumerable

        protected Result<IEnumerable<T>> Ok<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Ok), result, message, objs);
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected Result<IEnumerable<T>> Created<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Created), result, message, objs);
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected Result<IEnumerable<T>> Accepted<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            _logger.LogTrace(nameof(BaseService.Accepted), result, message, objs);
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected async Task<Result<T>> ExecuteAsync<T>(Func<Task<Result<T>>> method)
        {
            try
            {
                return await method();
            }
            catch (SheaftException e)
            {
                LogError(method, e);
                return Failed<T>(e);
            }
            catch (NotSupportedException e)
            {
                LogError(method, e);
                return Failed<T>(e);
            }
            catch (InvalidOperationException e)
            {
                LogError(method, e);
                return Failed<T>(e);
            }
            catch (Exception e)
            {
                LogError(method, e);
                return InternalError<T>();
            }
        }

        protected async Task<Result<IEnumerable<T>>> ExecuteAsync<T>(Func<Task<Result<IEnumerable<T>>>> method)
        {
            try
            {
                return await method();
            }
            catch (SheaftException e)
            {
                LogError(method, e);
                return Failed<IEnumerable<T>>(e);
            }
            catch (NotSupportedException e)
            {
                LogError(method, e);
                return Failed<IEnumerable<T>>(e);
            }
            catch (InvalidOperationException e)
            {
                LogError(method, e);
                return Failed<IEnumerable<T>>(e);
            }
            catch (Exception e)
            {
                LogError(method, e);
                return InternalError<IEnumerable<T>>();
            }
        }

        private void LogError<T>(Func<Task<Result<T>>> method, Exception e)
        {
            _logger.LogError($"{method.Method.DeclaringType?.DeclaringType?.Name}.{method.Method.Name.Split('>')[0].Replace("<", "")} : exception: {{0}}, message: {{1}}", e, e.Message);
        }

        private void LogError<T>(Func<Task<Result<IEnumerable<T>>>> method, Exception e)
        {
            _logger.LogError($"{method.Method.DeclaringType?.DeclaringType?.Name}.{method.Method.Name.Split('>')[0].Replace("<", "")} : exception: {{0}}, message: {{1}}", e, e.Message);
        }
    }
}
