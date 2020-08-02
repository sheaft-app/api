using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sheaft.Exceptions
{
    public class LockedException : SheaftException
    {
        public LockedException() : this(new KeyValuePair<MessageKind, object[]>())
        {
        }

        public LockedException(MessageKind message, params object[] args) : this(new KeyValuePair<MessageKind, object[]>(message, args))
        {
        }

        public LockedException(KeyValuePair<MessageKind, object[]> message) : base(ExceptionKind.Locked, message)
        {
        }
    }
}
