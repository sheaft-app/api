using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class LockedException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public LockedException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.Locked, exception, error, args)
        {
        }
        public LockedException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
