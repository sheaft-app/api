using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Sheaft.Exceptions;
using Sheaft.Interop.Enums;

namespace Sheaft.Core
{
    public class ResultsHandler
    {
        protected readonly ILogger Logger;

        public ResultsHandler(ILogger logger)
        {
            Logger = logger;
        }

        protected Result<T> Ok<T>(T result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Ok), result, message, objs);
            return new SuccessResult<T>(result, message, objs);
        }
        protected Result<T> ValidationError<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.ValidationError), message, objs);
            return new FailedResult<T>(new ValidationException(message, objs));
        }
        protected Result<T> BadRequest<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.BadRequest), message, objs);
            return new FailedResult<T>(new BadRequestException(message, objs));
        }
        protected Result<T> Conflict<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Conflict), message, objs);
            return new FailedResult<T>(new ConflictException(message, objs));
        }

        protected Result<T> Unauthorized<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Unauthorized), message, objs);
            return new FailedResult<T>(new UnauthorizedException(message, objs));
        }

        protected Result<T> Forbidden<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Forbidden), message, objs);
            return new FailedResult<T>(new ForbiddenException(message, objs));
        }

        protected Result<T> NotFound<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.NotFound), message, objs);
            return new FailedResult<T>(new NotFoundException(message, objs));
        }

        protected Result<T> Created<T>(T result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Created), result, message, objs);
            return new SuccessResult<T>(result, message, objs);
        }

        protected Result<T> Accepted<T>(T result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Accepted), result, message);
            return new SuccessResult<T>(result, message);
        }

        protected Result<T> Locked<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Locked), message, objs);
            return new FailedResult<T>(new LockedException(message, objs));
        }

        protected Result<T> InternalError<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.InternalError), message, objs);
            return new FailedResult<T>(new UnexpectedException(message, objs));
        }

        protected Result<T> Failed<T>(Exception exception, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Failed), exception, message, objs);
            return new FailedResult<T>(exception, message, objs);
        }

        protected Result<T> Failed<T>(SheaftException exception, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Failed), exception, message, objs);
            return new FailedResult<T>(exception, message, objs);
        }

        // IEnumerable

        protected Result<IEnumerable<T>> Ok<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Ok), result, message, objs);
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected Result<IEnumerable<T>> Created<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Created), result, message, objs);
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected Result<IEnumerable<T>> Accepted<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(ResultsHandler.Accepted), result, message, objs);
            return new SuccessResult<IEnumerable<T>>(result, message, objs);
        }

        protected async Task<Result<T>> ExecuteAsync<T>(Func<Task<Result<T>>> method)
        {
            try
            {
                Logger.LogTrace($"{nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}");
                return await method();
            }
            catch (SheaftException e)
            {
                Logger.LogError($"{nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return Failed<T>(e);
            }
            catch (Exception e)
            {
                Logger.LogCritical($"{nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return InternalError<T>();
            }
        }

        protected async Task<Result<IEnumerable<T>>> ExecuteAsync<T>(Func<Task<Result<IEnumerable<T>>>> method)
        {
            try
            {
                Logger.LogTrace($"{nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}");
                return await method();
            }
            catch (SheaftException e)
            {
                Logger.LogError($"{nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return Failed<IEnumerable<T>>(e);
            }
            catch (Exception e)
            {
                Logger.LogError($"{nameof(ResultsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return InternalError<IEnumerable<T>>();
            }
        }
    }
}