using Sheaft.Interop.Enums;
using System.Collections.Generic;

namespace Sheaft.Exceptions
{
    public class ValidationException : SheaftException
    {
        public ValidationException(MessageKind message, params object[] args) : this(new KeyValuePair<MessageKind, object[]>(message, args))
        {
        }

        public ValidationException(KeyValuePair<MessageKind, object[]> message) : base(ExceptionKind.Validation, message)
        {
        }
    }
}
