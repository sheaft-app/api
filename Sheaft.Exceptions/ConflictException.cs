using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Exceptions
{
    public class ConflictException : SheaftException
    {
        public ConflictException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.Conflict, exception, error, args)
        {
        }
        public ConflictException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
