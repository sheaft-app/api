﻿using Sheaft.Domain.Enum;

namespace Sheaft.Domain.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class ValidationException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public ValidationException(MessageKind? error, params object[] args) : base(ExceptionKind.Validation, null, error, args)
        {
        }
    }
}
