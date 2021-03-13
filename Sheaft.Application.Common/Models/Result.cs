using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models
{
    public class Result
    {
        internal Result(bool succeeded, MessageKind message, Exception exception = null, params object[] objs)
        {
            Succeeded = succeeded;
            Message = message;
            Exception = exception;
            Params = objs;
        }

        public bool Succeeded { get; }
        public MessageKind Message { get; }
        public Exception Exception { get; }
        public object[] Params { get; }

        public static Result Success(MessageKind? message = null, params object[] objs)
        {
            return new Result(true, message ?? MessageKind.Success, null, objs);
        }

        public static Result Failure(MessageKind? error, params object[] objs)
        {
            return new Result(false, error ?? MessageKind.Unexpected, null, objs);
        }

        public static Result Failure(MessageKind? error, Exception exception, params object[] objs)
        {
            return new Result(false, error ?? MessageKind.Unexpected, exception, objs);
        }
    }
}