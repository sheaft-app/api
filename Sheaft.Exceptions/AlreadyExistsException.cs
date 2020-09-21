using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class AlreadyExistsException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public AlreadyExistsException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.AlreadyExists, exception, error, args)
        {
        }
        public AlreadyExistsException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
