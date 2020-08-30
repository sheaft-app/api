using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Exceptions
{
    public class BadRequestException : SheaftException
    {
        public BadRequestException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.BadRequest, exception, error, args)
        {
        }
        public BadRequestException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
