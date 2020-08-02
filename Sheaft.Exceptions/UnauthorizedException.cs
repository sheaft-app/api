using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sheaft.Exceptions
{
    public class UnauthorizedException : SheaftException
    {
        public UnauthorizedException() : this(new KeyValuePair<MessageKind, object[]>())
        {
        }

        public UnauthorizedException(MessageKind message, params object[] args) : this(new KeyValuePair<MessageKind, object[]>(message, args))
        {
        }

        public UnauthorizedException(KeyValuePair<MessageKind, object[]> message) : base(ExceptionKind.Unauthorized, message)
        {
        }
    }
}
