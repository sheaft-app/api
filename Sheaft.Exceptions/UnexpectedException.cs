using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class UnexpectedException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public UnexpectedException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.Unexpected, exception, error, args)
        {
        }
        public UnexpectedException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
