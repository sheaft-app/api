using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class ConflictException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public ConflictException(Exception exception, params object[] args) : base(ExceptionKind.Conflict, exception, null, args)
        {
        }
        public ConflictException(params object[] args) : this(null, args)
        {
        }
    }
}
