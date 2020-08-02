using Sheaft.Interop.Enums;
using System.Collections.Generic;
using System.Net;

namespace Sheaft.Exceptions
{
    public class NotFoundException : SheaftException
    {
        public NotFoundException() : this(new KeyValuePair<MessageKind, object[]>())
        {
        }

        public NotFoundException(MessageKind message, params object[] args) : this(new KeyValuePair<MessageKind, object[]>(message, args))
        {
        }

        public NotFoundException(KeyValuePair<MessageKind, object[]> message) : base(ExceptionKind.NotFound, message)
        {
        }
    }
}
