using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sheaft.Exceptions
{
    public class SheaftException : Exception
    {
        public ExceptionKind Kind { get; private set; }
        public IEnumerable<KeyValuePair<MessageKind, object[]>> Errors { get; private set; }


        public SheaftException(ExceptionKind kind) : this(kind, new KeyValuePair<MessageKind, object[]>())
        {
        }
        public SheaftException(ExceptionKind kind, MessageKind error) : this(kind, new KeyValuePair<MessageKind, object[]>(error, null))
        {
        }

        public SheaftException(ExceptionKind kind, KeyValuePair<MessageKind, object[]> error) : this(kind, null, new List<KeyValuePair<MessageKind, object[]>> { error })
        {
        }

        public SheaftException(ExceptionKind kind, IEnumerable<KeyValuePair<MessageKind, object[]>> errors) : this(kind, null, errors)
        {
        }

        public SheaftException(ExceptionKind kind, Exception exception) : this(kind, exception, null)
        {
        }

        public SheaftException(ExceptionKind kind, Exception exception, KeyValuePair<MessageKind, object[]> error) : this(kind, exception, new List<KeyValuePair<MessageKind, object[]>> { error })
        {
        }

        public SheaftException(ExceptionKind kind, Exception exception, IEnumerable<KeyValuePair<MessageKind, object[]>> errors) : base(exception?.Message, exception)
        {
            Kind = kind;
            Errors = errors ?? new List<KeyValuePair<MessageKind, object[]>>();
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                switch (Kind)
                {
                    case ExceptionKind.BadRequest:
                    case ExceptionKind.Validation:
                        return HttpStatusCode.BadRequest;
                    case ExceptionKind.Conflict:
                        return HttpStatusCode.Conflict;
                    case ExceptionKind.Forbidden:
                        return HttpStatusCode.Forbidden;
                    case ExceptionKind.Gone:
                        return HttpStatusCode.Gone;
                    case ExceptionKind.Locked:
                        return HttpStatusCode.Locked;
                    case ExceptionKind.NotFound:
                        return HttpStatusCode.NotFound;
                    case ExceptionKind.Unauthorized:
                        return HttpStatusCode.Unauthorized;
                    default:
                        return HttpStatusCode.InternalServerError;
                }
            }
        }
    }
}
