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

        public bool Success { get; }
        public SheaftException Exception { get; }
        public KeyValuePair<MessageKind, object[]> Message { get; }

        // with result
        
        public CommandResult(bool success, T result) : this(success, new KeyValuePair<MessageKind, object[]>(), null)
        {
            Result = result;
        }

        public CommandResult(bool success, T result, MessageKind message) : this(success, new KeyValuePair<MessageKind, object[]>(message, null))
        {
            Result = result;
        }

        public CommandResult(bool success, T result, KeyValuePair<MessageKind, object[]> message) : this(success, message, null)
        {
            Result = result;
        }

        // without result

        public CommandResult(bool success, MessageKind message) : this(success, new KeyValuePair<MessageKind, object[]>(message, null))
        {
        }

        public CommandResult(bool success, KeyValuePair<MessageKind, object[]> message) : this(success, message, null)
        {
        }

        // exception result

        public CommandResult(SheaftException exception) : this(false, new KeyValuePair<MessageKind, object[]>(), exception)
        {
        }
        public CommandResult(Exception exception) : this(false, new KeyValuePair<MessageKind, object[]>(), new SheaftException(ExceptionKind.Unexpected, exception))
        {
        }
        public CommandResult(MessageKind message, Exception exception) : this(false, new KeyValuePair<MessageKind, object[]>(message, null), new SheaftException(ExceptionKind.Unexpected, exception))
        {
        }
        public CommandResult(MessageKind message, SheaftException exception) : this(false, new KeyValuePair<MessageKind, object[]>(message, null), exception)
        {
        }
        public CommandResult(KeyValuePair<MessageKind, object[]> message, SheaftException exception) : this(false, message, exception)
        {
        }


        protected CommandResult(bool success, KeyValuePair<MessageKind, object[]> message, SheaftException exception)
        {
            Success = success;
            Message = message;
            Exception = exception;
        }
    }
}