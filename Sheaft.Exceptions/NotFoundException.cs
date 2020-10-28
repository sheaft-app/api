using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class NotFoundException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public NotFoundException(params object[] args) : base(ExceptionKind.NotFound, null, null, args)
        {
        }
    }
}
