using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class UnauthorizedException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public UnauthorizedException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.Unauthorized, exception, error, args)
        {
        }
        public UnauthorizedException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
