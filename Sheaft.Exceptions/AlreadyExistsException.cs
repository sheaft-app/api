using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class AlreadyExistsException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public AlreadyExistsException(params object[] args) : base(ExceptionKind.AlreadyExists, null, null, args)
        {
        }
    }
}
