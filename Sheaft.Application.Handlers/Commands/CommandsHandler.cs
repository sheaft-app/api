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

        protected CommandResult<T> Ok<T>(T result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Ok), result, message, objs);
            return new CommandResult<T>(result, message, objs);
        }
        protected CommandResult<T> ValidationError<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.ValidationError), message, objs);
            return new CommandResult<T>(new ValidationException(message, objs));
        }
        protected CommandResult<T> BadRequest<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.BadRequest), message, objs);
            return new CommandResult<T>(new BadRequestException(message, objs));
        }
        protected CommandResult<T> Conflict<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Conflict), message, objs);
            return new CommandResult<T>(new ConflictException(message, objs));
        }

        protected CommandResult<T> Unauthorized<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Unauthorized), message, objs);
            return new CommandResult<T>(new UnauthorizedException(message, objs));
        }

        protected CommandResult<T> Forbidden<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Forbidden), message, objs);
            return new CommandResult<T>(new ForbiddenException(message, objs));
        }

        protected CommandResult<T> NotFound<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.NotFound), message, objs);
            return new CommandResult<T>(new NotFoundException(message, objs));
        }

        protected CommandResult<T> Created<T>(T result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Created), result, message, objs);
            return new CommandResult<T>(result, message, objs);
        }

        protected CommandResult<T> Accepted<T>(T result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Accepted), result, message);
            return new CommandResult<T>(result, message);
        }

        protected CommandResult<T> Locked<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Locked), message, objs);
            return new CommandResult<T>(new LockedException(message, objs));
        }

        protected CommandResult<T> InternalError<T>(MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.InternalError), message, objs);
            return new CommandResult<T>(new UnexpectedException(message, objs));
        }

        protected CommandResult<T> Failed<T>(SheaftException exception, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Failed), exception, message, objs);
            return new CommandResult<T>(exception, message, objs);
        }

        // IEnumerable

        protected CommandResult<IEnumerable<T>> Ok<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Ok), result, message, objs);
            return new CommandResult<IEnumerable<T>>(result, message, objs);
        }

        protected CommandResult<IEnumerable<T>> Created<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Created), result, message, objs);
            return new CommandResult<IEnumerable<T>>(result, message, objs);
        }

        protected CommandResult<IEnumerable<T>> Accepted<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            Logger.LogTrace(nameof(CommandsHandler.Accepted), result, message, objs);
            return new CommandResult<IEnumerable<T>>(result, message, objs);
        }

        protected async Task<CommandResult<T>> ExecuteAsync<T>(Func<Task<CommandResult<T>>> method)
        {
            try
            {
                Logger.LogTrace(nameof(CommandsHandler.ExecuteAsync), method);
                return await method();
            }
            catch (SheaftException e)
            {
                Logger.LogError(nameof(CommandsHandler.ExecuteAsync), e);
                return Failed<T>(e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogError(nameof(CommandsHandler.ExecuteAsync), e);
                return Conflict<T>();
            }
            catch (Exception e)
            {
                Logger.LogError(nameof(CommandsHandler.ExecuteAsync), e);
                return InternalError<T>();
            }
        }

        protected async Task<CommandResult<IEnumerable<T>>> ExecuteAsync<T>(Func<Task<CommandResult<IEnumerable<T>>>> method)
        {
            try
            {
                Logger.LogTrace(nameof(CommandsHandler.ExecuteAsync), method);
                return await method();
            }
            catch (SheaftException e)
            {
                Logger.LogError(nameof(CommandsHandler.ExecuteAsync), e);
                return Failed<IEnumerable<T>>(e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogError(nameof(CommandsHandler.ExecuteAsync), e);
                return Conflict<IEnumerable<T>>();
            }
            catch (Exception e)
            {
                Logger.LogError(nameof(CommandsHandler.ExecuteAsync), e);
                return InternalError<IEnumerable<T>>();
            }
        }
    }
}