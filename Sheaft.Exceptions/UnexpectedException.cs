using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sheaft.Exceptions
{
    public class UnexpectedException : SheaftException
    {
        public UnexpectedException(Exception exception = null) : this(new KeyValuePair<MessageKind, object[]>(), exception)
        {
        }

        public UnexpectedException(MessageKind message, Exception exception = null, params object[] args) : this(new KeyValuePair<MessageKind, object[]>(message, args), exception)
        {
        }

        public UnexpectedException(KeyValuePair<MessageKind, object[]> message, Exception exception = null) : base(ExceptionKind.Unexpected, exception, message)
        {
        }
    }
}
