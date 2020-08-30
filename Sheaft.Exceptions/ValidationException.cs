using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Exceptions
{
    public class ValidationException : SheaftException
    {
        public ValidationException(Exception exception, MessageKind? error = null, params object[] args): base(ExceptionKind.Validation, exception, error, args)
        {
        }
        public ValidationException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
