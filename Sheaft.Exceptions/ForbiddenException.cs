using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sheaft.Exceptions
{
    public class ForbiddenException : SheaftException
    {
        public ForbiddenException() : this(new KeyValuePair<MessageKind, object[]>())
        {
        }

        public ForbiddenException(MessageKind message, params object[] args) : this(new KeyValuePair<MessageKind, object[]>(message, args))
        {
        }

        public ForbiddenException(KeyValuePair<MessageKind, object[]> message) : base(ExceptionKind.Forbidden, message)
        {
        }
    }
}
