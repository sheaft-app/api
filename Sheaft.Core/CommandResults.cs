using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sheaft.Core
{
    public class CommandResult<T>
    {
        public T Result { get; }

        public bool Success => Result != null;
        public SheaftException Exception { get; }
        public MessageKind? Message { get; }
        public object[] Params { get; }

        // with result
        
        public CommandResult(T result)
        {
            Result = result;
        }

        public CommandResult(T result, MessageKind? message = null, params object[] objs) : this(message, objs)
        {
            Result = result;
        }

        // exception result

        public CommandResult(Exception exception, MessageKind? message = null, params object[] objs) : this(new SheaftException(ExceptionKind.Unexpected, exception), message, objs)
        {
        }
        public CommandResult(SheaftException exception, MessageKind? message = null, params object[] objs) : this(message ?? MessageKind.Unexpected, objs)
        {
            Exception = exception;
        }

        protected CommandResult(MessageKind? message, params object[] objs)
        {
            Message = message;
            Params = objs;
        }
    }
}