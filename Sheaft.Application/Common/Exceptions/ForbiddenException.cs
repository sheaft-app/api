using System;
using Sheaft.Domain.Enums;
using Sheaft.Domains.Exceptions;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class ForbiddenException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public ForbiddenException(params object[] args) : base(ExceptionKind.Forbidden, null, null, args)
        {
        }
    }
}
