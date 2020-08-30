using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Exceptions
{
    public class NotFoundException : SheaftException
    {
        public NotFoundException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.NotFound, exception, error, args)
        {
        }
        public NotFoundException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
