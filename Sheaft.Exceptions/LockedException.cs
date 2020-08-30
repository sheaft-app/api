using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Exceptions
{
    public class LockedException : SheaftException
    {
        public LockedException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.Locked, exception, error, args)
        {
        }
        public LockedException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
