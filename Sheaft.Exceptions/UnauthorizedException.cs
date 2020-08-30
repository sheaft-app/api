using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Exceptions
{
    public class UnauthorizedException : SheaftException
    {
        public UnauthorizedException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.Unauthorized, exception, error, args)
        {
        }
        public UnauthorizedException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
