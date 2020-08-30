using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sheaft.Exceptions
{
    public class UnexpectedException : SheaftException
    {
        public UnexpectedException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.Unexpected, exception, error, args)
        {
        }
        public UnexpectedException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
