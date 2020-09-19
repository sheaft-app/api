using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Exceptions
{
    public class TooManyRetriesException : SheaftException
    {
        public TooManyRetriesException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.TooManyRetries, exception, error, args)
        {
        }
        public TooManyRetriesException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
