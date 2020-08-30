using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Exceptions
{
    public class ForbiddenException : SheaftException
    {
        public ForbiddenException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.Forbidden, exception, error, args)
        {
        }
        public ForbiddenException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
