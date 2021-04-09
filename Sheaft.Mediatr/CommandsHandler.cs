using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;

namespace Sheaft.Mediatr
{
    public abstract class CommandsHandler
    {
        protected readonly ILogger _logger;
        protected readonly ISheaftMediatr _mediatr;
        protected readonly IAppDbContext _context;

        protected CommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger logger)
        {
            _mediatr = mediatr;
            _context = context;
            _logger = logger;
        }

        protected Result Success()
        {
            return Result.Success();
        }

        protected Result Success(MessageKind message, params object[] objs)
        {
            return Result.Success(message, objs);
        }

        protected Result Failure(MessageKind message, params object[] objs)
        {
            return Failure(null, message, objs);
        }

        protected Result Failure(Exception exception, params object[] objs)
        {
            return Failure(exception, null, objs);
        }

        protected Result Failure(Exception exception, MessageKind message, params object[] objs)
        {
            return Result.Failure(message, exception, objs);
        }

        protected Result<T> Success<T>(T result)
        {
            return Result<T>.Success(result);
        }

        protected Result<T> Success<T>(T result, MessageKind message, params object[] objs)
        {
            return Result<T>.Success(result, message, objs);
        }

        protected Result<T> Failure<T>(Exception exception, params object[] objs)
        {
            return Failure<T>(exception, null, objs);
        }

        protected Result<T> Failure<T>(MessageKind message, params object[] objs)
        {
            return Failure<T>(null, message, objs);
        }

        protected Result<T> Failure<T>(Exception exception, MessageKind message, params object[] objs)
        {
            return Result<T>.Failure(default(T), message, exception, objs);
        }
    }
}