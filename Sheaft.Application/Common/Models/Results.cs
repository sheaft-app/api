using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models
{
    public class Result<T> : Result
    {
        public T Data { get; private set; }

        internal Result(bool succeeded, MessageKind message, Exception exception = null, params object[] objs) : base(succeeded, message, exception, objs)
        {
        }
        
        public static Result<T> Success(T data, MessageKind? message = null, params object[] objs)
        {
            return new Result<T>(true, message ?? MessageKind.Success, null, objs) {Data = data};
        }

        public static Result<T> Failure(T data, MessageKind? error, params object[] objs)
        {
            return new Result<T>(false, error ?? MessageKind.Unexpected, null, objs);
        }

        public static Result<T> Failure(T data, MessageKind? error, Exception exception, params object[] objs)
        {
            return new Result<T>(false, error ?? MessageKind.Unexpected, exception, objs);
        }
    }
}