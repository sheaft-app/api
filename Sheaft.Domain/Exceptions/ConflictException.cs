using System;

namespace Sheaft.Domain.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(Exception exception)
            : this(exception.Message, exception)
        {
        }

        public ConflictException(string message, Exception exception = null)
            : base(message, exception)
        {
        }
    }
}