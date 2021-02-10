using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Services
{
    public class BaseService
    {
        protected readonly ILogger _logger;

        public BaseService(ILogger logger)
        {
            _logger = logger;
        }

        protected Result Success(MessageKind? message = null, params object[] objs)
        {
            return Result.Success(message, objs);
        }

        protected Result<T> Success<T>(T result, MessageKind? message = null, params object[] objs)
        {
            return Result<T>.Success(result, message, objs);
        }

        protected Result Failure(MessageKind? message = null, params object[] objs)
        {
            return Failure(null, message, objs);
        }

        protected Result Failure(Exception exception, MessageKind? message = null, params object[] objs)
        {
            return Result.Failure(message, exception, objs);
        }

        protected Result<T> Failure<T>(MessageKind? message = null, params object[] objs)
        {
            return Failure<T>(null, message, objs);
        }

        protected Result<T> Failure<T>(Exception exception, MessageKind? message = null, params object[] objs)
        {
            return Result<T>.Failure(default(T), message, exception, objs);
        }

        protected Result<IEnumerable<T>> Success<T>(IEnumerable<T> result, MessageKind? message = null, params object[] objs)
        {
            return Result<IEnumerable<T>>.Success(result, message, objs);
        }
    }
}
