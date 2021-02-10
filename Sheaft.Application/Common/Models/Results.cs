using Sheaft.Exceptions;
using System;
using Sheaft.Domain.Enums;
using Sheaft.Domains.Exceptions;

namespace Sheaft.Core
{
    public class Result<T>
    {
        public T Data { get; }
        public bool Success { get; }
        public SheaftException Exception { get; }
        public MessageKind? Message { get; }
        public object[] Params { get; }

        // with result

        protected Result(bool success, T data, MessageKind? message = null, params object[] objs) : this(message, objs)
        {
            Success = success;
            Data = data;
        }

        // exception result

        protected Result(Exception exception, MessageKind? message = null, params object[] objs) : this(new UnexpectedException(exception), message, objs)
        {
        }
        protected Result(SheaftException exception, MessageKind? message = null, params object[] objs) : this(message ?? MessageKind.Unexpected, objs)
        {
            Success = false;
            Exception = exception;
        }

        protected Result(MessageKind? message, params object[] objs)
        {
            Message = message;
            Params = objs;
        }
    }

    public class SuccessResult<T> : Result<T>
    {
        public SuccessResult(T data, MessageKind? message = null, params object[] objs)
            :base(true, data, message, objs)
        {
        }
    }

    public class FailedResult<T> : Result<T>
    {
        public FailedResult(Exception e, MessageKind? message = null, params object[] objs)
            : base(e, message, objs)
        {
        }
        public FailedResult(SheaftException e, MessageKind? message = null, params object[] objs)
            : base(e, message, objs)
        {
        }
    }
}