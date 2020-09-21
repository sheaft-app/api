using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class ValidationException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public ValidationException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.Validation, exception, error, args)
        {
        }
        public ValidationException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
