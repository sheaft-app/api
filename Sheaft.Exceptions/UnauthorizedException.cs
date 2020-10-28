using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class UnauthorizedException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public UnauthorizedException(params object[] args) : base(ExceptionKind.Unauthorized, null, null, args)
        {
        }
    }
}
