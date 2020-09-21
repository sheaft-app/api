using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class NotFoundException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public NotFoundException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.NotFound, exception, error, args)
        {
        }
        public NotFoundException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
