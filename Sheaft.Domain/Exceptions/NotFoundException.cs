using System;

namespace Sheaft.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public string Resource { get; }
        public Guid? Id { get; }

        public NotFoundException(Guid? id, string resource = null, string message = null)
            : this(message)
        {
            Resource = resource;
            Id = id;
        }
        
        public NotFoundException(Exception exception)
            : this(exception.Message, exception)
        {
        }
        
        public NotFoundException(string message, Exception exception = null)
            : base(message, exception)
        {
        }
    }
}