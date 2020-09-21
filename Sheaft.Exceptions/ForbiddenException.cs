using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class ForbiddenException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public ForbiddenException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.Forbidden, exception, error, args)
        {
        }
        public ForbiddenException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
