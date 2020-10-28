using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class TooManyRetriesException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public TooManyRetriesException(Exception exception, params object[] args) : base(ExceptionKind.TooManyRetries, exception, null, args)
        {
        }
        public TooManyRetriesException(params object[] args) : this(null, args)
        {
        }
    }
}
