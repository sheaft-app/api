using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Sheaft.Exceptions;
using System.Collections.Generic;
using Sheaft.Domain.Enums;
using Sheaft.Domains.Exceptions;

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

        protected Result<T> InternalError<T>(Exception e, MessageKind? message = null, params object[] objs)
        {
            return new FailedResult<T>(new UnexpectedException(e, message, objs));
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


        protected Result<T> HandleException<T>(string method, Exception e)
        {
            if (e is SheaftException sheaftException)
            {
                _logger.LogError(sheaftException, $"Error on executing {method} : {sheaftException.Message}");
                return Failed<T>(sheaftException.InnerException ?? sheaftException);
            }

            if (e is NotSupportedException notSupported)
            {
                _logger.LogError(notSupported, $"Error on executing {method} : {notSupported.Message}");
                return Failed<T>(notSupported.InnerException ?? notSupported);
            }

            if (e is InvalidOperationException invalidOperation)
            {
                _logger.LogError(invalidOperation, $"Error on executing {method} : {invalidOperation.Message}");
                return Failed<T>(invalidOperation.InnerException ?? invalidOperation);
            }

            _logger.LogError(e, $"Error on executing {method} : {e.Message}");
            return InternalError<T>(e.InnerException ?? e);
        }

        protected async Task<Result<T>> ExecuteAsync<T>(Func<Task<Result<T>>> method)
        {
            var type = method.Method.Name.Split('>')[0].Replace("<", string.Empty)+ "()";

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
                return HandleException<T>(type, e);
            }
        }

        protected async Task<Result<IEnumerable<T>>> ExecuteAsync<T>(object request, Func<Task<Result<IEnumerable<T>>>> method)
        {
            var type = method.Method.Name.Split('>')[0].Replace("<", string.Empty) + "()";

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
                return HandleException<IEnumerable<T>>(type, e);
            }
        }
    }
}
