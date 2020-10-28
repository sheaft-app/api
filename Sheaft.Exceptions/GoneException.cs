using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class GoneException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public GoneException(params object[] args) : base(ExceptionKind.Gone, null, null, args)
        {
        }
    }
}
