using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class TooManyRetriesException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public TooManyRetriesException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.TooManyRetries, exception, error, args)
        {
        }
        public TooManyRetriesException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
