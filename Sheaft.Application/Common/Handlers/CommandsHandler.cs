using System;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Handlers
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

        protected Result Success(MessageKind? message = null, params object[] objs)
        {
            return Result.Success(message, objs);
        }

        protected Result Failure(MessageKind? message = null, params object[] objs)
        {
            return Failure(null, message, objs);
        }

        protected Result Failure(Exception exception, MessageKind? message = null, params object[] objs)
        {
            return Result.Failure(message, exception, objs);
        }

        protected Result<T> Success<T>(T result, MessageKind? message = null, params object[] objs)
        {
            return Result<T>.Success(result, message, objs);
        }

        protected Result<T> Failure<T>(MessageKind? message = null, params object[] objs)
        {
            return Failure<T>(null, message, objs);
        }

        protected Result<T> Failure<T>(Exception exception, MessageKind? message = null, params object[] objs)
        {
            return Result<T>.Failure(default(T), message, exception, objs);
        }
    }
}