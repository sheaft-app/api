using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Exceptions
{
    public class SheaftException : Exception
    {
        public ExceptionKind Kind { get; }
        public MessageKind? Error { get; }
        public object[] Params { get; }

        public SheaftException(ExceptionKind? kind = null, MessageKind? error = null, params object[] objs) : this(kind ?? ExceptionKind.Unexpected, null, error, objs)
        {
        }

        public SheaftException(ExceptionKind kind, Exception exception, MessageKind? error = null, params object[] objs) : base(exception?.Message, exception)
        {
            Kind = kind;
            Error = error;
            Params = objs;
        }
    }
}
