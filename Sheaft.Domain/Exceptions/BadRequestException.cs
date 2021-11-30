using System;

namespace Sheaft.Domain.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(Exception exception)
            : this(exception.Message, exception)
        {
        }

        public BadRequestException(string message, Exception exception = null)
            : base(message, exception)
        {
        }
    }
}