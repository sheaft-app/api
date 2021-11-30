using System;

namespace Sheaft.Domain.Exceptions
{
    public class UnexpectedException : Exception
    {
        public UnexpectedException(Exception exception)
            : this(exception?.Message, exception)
        {
        }
        
        public UnexpectedException(string message, Exception exception = null)
            : base(message, exception)
        {
        }
    }
}