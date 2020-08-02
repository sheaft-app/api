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

        protected CommandResult<T> OkResult<T>(T result, MessageKind? message = null)
        {
            return OkResult<T>(result, message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>());
        }
        protected CommandResult<T> OkResult<T>(T result, KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.OkResult), result, message);
            return new CommandResult<T>(true, result, message);
        }
        protected CommandResult<T> ValidationResult<T>(MessageKind? message = null)
        {
            return ValidationResult<T>(message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>(MessageKind.Validation, null));
        }
        protected CommandResult<T> ValidationResult<T>(KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.ValidationResult), message);
            return new CommandResult<T>(false, message);
        }
        protected CommandResult<T> BadRequestResult<T>(MessageKind? message = null)
        {
            return BadRequestResult<T>(message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>(MessageKind.BadRequest, null));
        }
        protected CommandResult<T> BadRequestResult<T>(KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.BadRequestResult), message);
            return new CommandResult<T>(false, message);
        }
        protected CommandResult<T> ConflictResult<T>(MessageKind? message = null)
        {
            return ConflictResult<T>(message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>(MessageKind.Conflict, null));
        }
        protected CommandResult<T> ConflictResult<T>(KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.ConflictResult), message);
            return new CommandResult<T>(false, message);
        }

        protected CommandResult<T> UnauthorizedResult<T>(MessageKind? message = null)
        {
            return UnauthorizedResult<T>(message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>(MessageKind.Unauthorized, null));
        }
        protected CommandResult<T> UnauthorizedResult<T>(KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.UnauthorizedResult), message);
            return new CommandResult<T>(false, message);
        }

        protected CommandResult<T> ForbiddenResult<T>(MessageKind? message = null)
        {
            return ForbiddenResult<T>(message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>(MessageKind.Forbidden, null));
        }
        protected CommandResult<T> ForbiddenResult<T>(KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.ForbiddenResult), message);
            return new CommandResult<T>(false, message);
        }

        protected CommandResult<T> NotFoundResult<T>(MessageKind? message = null)
        {
            return NotFoundResult<T>(message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>(MessageKind.NotFound, null));
        }
        protected CommandResult<T> NotFoundResult<T>(KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.NotFoundResult), message);
            return new CommandResult<T>(false, message);
        }

        protected CommandResult<T> CreatedResult<T>(T result, MessageKind? message = null)
        {
            return CreatedResult<T>(result, message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>());
        }
        protected CommandResult<T> CreatedResult<T>(T result, KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.CreatedResult), result, message);
            return new CommandResult<T>(true, result, message);
        }

        protected CommandResult<T> AcceptedResult<T>(T result, MessageKind? message = null)
        {
            return AcceptedResult<T>(result, message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>());
        }
        protected CommandResult<T> AcceptedResult<T>(T result, KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.AcceptedResult), result, message);
            return new CommandResult<T>(true, result, message);
        }

        protected CommandResult<T> LockedResult<T>(MessageKind? message = null)
        {
            return LockedResult<T>(message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>(MessageKind.Locked, null));
        }
        protected CommandResult<T> LockedResult<T>(KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.LockedResult), message);
            return new CommandResult<T>(false, message);
        }

        protected CommandResult<T> InternalErrorResult<T>(MessageKind? message = null)
        {
            return InternalErrorResult<T>(message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>(MessageKind.Unexpected, null));
        }
        protected CommandResult<T> InternalErrorResult<T>(KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.InternalErrorResult), message);
            return new CommandResult<T>(false, message);
        }


        protected CommandResult<T> CommandFailed<T>(SheaftException exception, MessageKind? message = null)
        {
            if (message.HasValue)
                return new CommandResult<T>(message.Value, exception);

            Logger.LogTrace(nameof(CommandsHandler.CommandFailed), exception, message);
            return new CommandResult<T>(exception);
        }

        // IEnumerable

        protected CommandResult<IEnumerable<T>> OkResult<T>(IEnumerable<T> result, MessageKind? message = null)
        {
            return OkResult<IEnumerable<T>>(result, message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>());
        }
        protected CommandResult<IEnumerable<T>> OkResult<T>(IEnumerable<T> result, KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.OkResult), result, message);
            return new CommandResult<IEnumerable<T>>(true, result, message);
        }

        protected CommandResult<IEnumerable<T>> CreatedResult<T>(IEnumerable<T> result, MessageKind? message = null)
        {
            return CreatedResult<IEnumerable<T>>(result, message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>());
        }
        protected CommandResult<IEnumerable<T>> CreatedResult<T>(IEnumerable<T> result, KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.CreatedResult), result, message);
            return new CommandResult<IEnumerable<T>>(true, result, message);
        }

        protected CommandResult<IEnumerable<T>> AcceptedResult<T>(IEnumerable<T> result, MessageKind? message = null)
        {
            return AcceptedResult<IEnumerable<T>>(result, message.HasValue ? new KeyValuePair<MessageKind, object[]>(message.Value, null) : new KeyValuePair<MessageKind, object[]>());
        }
        protected CommandResult<IEnumerable<T>> AcceptedResult<T>(IEnumerable<T> result, KeyValuePair<MessageKind, object[]> message)
        {
            Logger.LogTrace(nameof(CommandsHandler.AcceptedResult), result, message);
            return new CommandResult<IEnumerable<T>>(true, result, message);
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
                return new CommandResult<T>(e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogError(nameof(CommandsHandler.ExecuteAsync), e);
                return new CommandResult<T>(MessageKind.Conflict, new ConflictException());
            }
            catch (Exception e)
            {
                Logger.LogError(nameof(CommandsHandler.ExecuteAsync), e);
                return new CommandResult<T>(MessageKind.Unexpected, new SheaftException(ExceptionKind.Unexpected, e));
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
                return new CommandResult<IEnumerable<T>>(e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogError(nameof(CommandsHandler.ExecuteAsync), e);
                return new CommandResult<IEnumerable<T>>(MessageKind.Conflict, new ConflictException());
            }
            catch (Exception e)
            {
                Logger.LogError(nameof(CommandsHandler.ExecuteAsync), e);
                return new CommandResult<IEnumerable<T>>(MessageKind.Unexpected, new SheaftException(ExceptionKind.Unexpected, e));
            }
        }
    }
}