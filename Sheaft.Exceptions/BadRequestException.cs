using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sheaft.Exceptions
{
    public class BadRequestException : SheaftException
    {
        public BadRequestException() : this(new KeyValuePair<MessageKind, object[]>())
        {
        }

        public BadRequestException(MessageKind message, params object[] args) : this(new KeyValuePair<MessageKind, object[]>(message, args))
        {
        }

        public BadRequestException(KeyValuePair<MessageKind, object[]> message) : base(ExceptionKind.BadRequest, message)
        {
        }
    }
}
