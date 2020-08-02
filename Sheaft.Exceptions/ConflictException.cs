using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sheaft.Exceptions
{
    public class ConflictException : SheaftException
    {
        public ConflictException() : this(new KeyValuePair<MessageKind, object[]>())
        {
        }

        public ConflictException(MessageKind message, params object[] args) : this(new KeyValuePair<MessageKind, object[]>(message, args))
        {
        }

        public ConflictException(KeyValuePair<MessageKind, object[]> message) : base(ExceptionKind.Conflict, message)
        {
        }
    }
}
