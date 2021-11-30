using System;
using System.Collections.Generic;

namespace Sheaft.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public Guid? Id { get; }
        public string Resource { get; }
        public IEnumerable<KeyValuePair<string, string>> Errors { get; }

        public ValidationException(IEnumerable<KeyValuePair<string, string>> errors, string message = null,
            Guid? id = null, string resource = null)
            : base(message)
        {
            Id = id;
            Resource = resource;
            Errors = errors ?? new List<KeyValuePair<string, string>>();
        }

        public ValidationException(Exception exception)
            : this(exception.Message, exception)
        {
        }

        public ValidationException(string message, Exception exception = null)
            : base(message, exception)
        {
            Errors = new List<KeyValuePair<string, string>> {new ("error", message)};
        }
    }
}