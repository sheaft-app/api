using System;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;

namespace Sheaft.Mediatr
{
    public abstract class CommandsHandler
    {
        protected readonly ILogger _logger;
        protected readonly ISheaftMediatr _mediatr;
        protected IAppDbContext _context;

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

        protected Result Success(string message, params object[] objs)
        {
            return Result.Success(message, objs);
        }

        protected Result Failure(Result result)
        {
            return Failure(result.Exception, result.Message, result.Params);
        }

        protected Result Failure(string message, params object[] objs)
        {
            return Failure(null, message, objs);
        }

        protected Result Failure(Exception exception, string message, params object[] objs)
        {
            return Result.Failure(message, exception, objs);
        }

        protected Result<T> Success<T>()
        {
            return Result<T>.Success(default(T));
        }

        protected Result<T> Success<T>(T result)
        {
            return Result<T>.Success(result);
        }

        protected Result<T> Success<T>(T result, string message, params object[] objs)
        {
            return Result<T>.Success(result, message, objs);
        }

        protected Result<T> Failure<T>(Result result)
        {
            return Failure<T>(result.Exception, result.Message, result.Params);
        }

        protected Result<T> Failure<T>(string message, params object[] objs)
        {
            return Failure<T>(null, message, objs);
        }

        protected Result<T> Failure<T>(Exception exception, string message, params object[] objs)
        {
            return Result<T>.Failure(default(T), message, exception, objs);
        }
    }
}