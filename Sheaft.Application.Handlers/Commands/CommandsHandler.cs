using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Sheaft.Exceptions;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Handlers
{
    public class CommandsHandler
    {
        protected readonly ILogger Logger;

        public CommandsHandler(ILogger logger)
        {
            Logger = logger;
        }

        protected Result<T> Ok<T>(T result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Ok), result, message, objs);
            return new Result<T>(result, message, objs);
        }
        protected Result<T> ValidationError<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.ValidationError), message, objs);
            return new Result<T>(new ValidationException(message, objs));
        }
        protected Result<T> BadRequest<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.BadRequest), message, objs);
            return new Result<T>(new BadRequestException(message, objs));
        }
        protected Result<T> Conflict<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Conflict), message, objs);
            return new Result<T>(new ConflictException(message, objs));
        }

        protected Result<T> Unauthorized<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Unauthorized), message, objs);
            return new Result<T>(new UnauthorizedException(message, objs));
        }

        protected Result<T> Forbidden<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Forbidden), message, objs);
            return new Result<T>(new ForbiddenException(message, objs));
        }

        protected Result<T> NotFound<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.NotFound), message, objs);
            return new Result<T>(new NotFoundException(message, objs));
        }

        protected Result<T> Created<T>(T result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Created), result, message, objs);
            return new Result<T>(result, message, objs);
        }

        protected Result<T> Accepted<T>(T result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Accepted), result, message);
            return new Result<T>(result, message);
        }

        protected Result<T> Locked<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Locked), message, objs);
            return new Result<T>(new LockedException(message, objs));
        }

        protected Result<T> InternalError<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.InternalError), message, objs);
            return new Result<T>(new UnexpectedException(message, objs));
        }

        protected Result<T> Failed<T>(SheaftException exception, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Failed), exception, message, objs);
            return new Result<T>(exception, message, objs);
        }

        // IEnumerable

        protected Result<IEnumerable<T>> Ok<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Ok), result, message, objs);
            return new Result<IEnumerable<T>>(result, message, objs);
        }

        protected Result<IEnumerable<T>> Created<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Created), result, message, objs);
            return new Result<IEnumerable<T>>(result, message, objs);
        }

        protected Result<IEnumerable<T>> Accepted<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Accepted), result, message, objs);
            return new Result<IEnumerable<T>>(result, message, objs);
        }

        protected async Task<Result<T>> ExecuteAsync<T>(Func<Task<Result<T>>> method)
        {
            try
            {
                Logger.LogTrace($"{nameof(CommandsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}");
                return await method();
            }
            catch (SheaftException e)
            {
                Logger.LogError($"{nameof(CommandsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return Failed<T>(e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogError($"{nameof(CommandsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return Conflict<T>();
            }
            catch (Exception e)
            {
                Logger.LogCritical($"{nameof(CommandsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return InternalError<T>();
            }
        }

        protected async Task<Result<IEnumerable<T>>> ExecuteAsync<T>(Func<Task<Result<IEnumerable<T>>>> method)
        {
            try
            {
                Logger.LogTrace($"{nameof(CommandsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}");
                return await method();
            }
            catch (SheaftException e)
            {
                Logger.LogError($"{nameof(CommandsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return Failed<IEnumerable<T>>(e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogError($"{nameof(CommandsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return Conflict<IEnumerable<T>>();
            }
            catch (Exception e)
            {
                Logger.LogError($"{nameof(CommandsHandler.ExecuteAsync)} - {method.Method.DeclaringType?.DeclaringType?.Name ?? method.Target.ToString().Split("+")[0]}: exception: {{0}}", e);
                return InternalError<IEnumerable<T>>();
            }
        }
    }
}