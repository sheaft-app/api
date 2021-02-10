using System;
using Sheaft.Domain.Enums;
using Sheaft.Domains.Exceptions;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class UnexpectedException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public UnexpectedException(Exception exception, MessageKind? message = null, params object[] args) : base(ExceptionKind.Unexpected, exception, message, args)
        {
        }
        public UnexpectedException(MessageKind? message = null, params object[] args) : this(null, message, args)
        {
        }
    }
}
