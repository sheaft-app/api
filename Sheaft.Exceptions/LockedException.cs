using System;

namespace Sheaft.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class LockedException : SheaftException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public LockedException(params object[] args) : base(ExceptionKind.Locked, null, null, args)
        {
        }
    }
}
