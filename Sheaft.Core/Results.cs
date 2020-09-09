using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Core
{
    public class Result<T>
    {
        public T Data { get; }

        public bool Success => Data != null;
        public SheaftException Exception { get; }
        public MessageKind? Message { get; }
        public object[] Params { get; }

        // with result
        
        public Result(T data, MessageKind? message = null, params object[] objs) : this(message, objs)
        {
            Data = data;
        }

        // exception result

        public Result(Exception exception, MessageKind? message = null, params object[] objs) : this(new SheaftException(ExceptionKind.Unexpected, exception), message, objs)
        {
        }
        public Result(SheaftException exception, MessageKind? message = null, params object[] objs) : this(message ?? MessageKind.Unexpected, objs)
        {
            Exception = exception;
        }

        protected Result(MessageKind? message, params object[] objs)
        {
            Message = message;
            Params = objs;
        }
    }
}