using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class BadRequestException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public BadRequestException(Exception exception, MessageKind? error = null, params object[] args) : base(ExceptionKind.BadRequest, exception, error, args)
        {
        }
        public BadRequestException(MessageKind? error = null, params object[] args) : this(null, error, args)
        {
        }
    }
}
