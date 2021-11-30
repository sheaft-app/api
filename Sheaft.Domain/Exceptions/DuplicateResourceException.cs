using System;

namespace Sheaft.Domain.Exceptions
{
    public class DuplicateResourceException : Exception
    {
        public DuplicateResourceException(Exception exception)
            : this(exception.Message, exception)
        {
        }

        public DuplicateResourceException(string message, Exception exception = null)
            : base(message, exception)
        {
        }
    }
}