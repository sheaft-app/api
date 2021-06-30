using System;
using Sheaft.Core.Enums;

namespace Sheaft.Core.Exceptions
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class SheaftException : Exception
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public ExceptionKind Kind { get; }
        public string Error { get; }
        public object[] Params { get; }
        
        public SheaftException(Result result) : this(ExceptionKind.Unexpected, result.Exception, result.Message, result.Params)
        {
        }
        
        protected SheaftException(ExceptionKind? kind = null, string error = null, params object[] objs) : this(kind ?? ExceptionKind.Unexpected, null, error, objs)
        {
        }

        protected SheaftException(ExceptionKind kind, Exception exception, string error = null, params object[] objs) : base(exception?.Message ?? $"{kind:G}{(error != null ? $":{error:G}":string.Empty)}", exception)
        {
            Kind = kind;
            Error = error;
            Params = objs;
        }

        public static SheaftException Conflict(string error = null, params object[] objs)
        {
            return Conflict(null, error, objs);
        }
        
        public static SheaftException Conflict(Exception exception, string error = null, params object[] objs)
        {
            return new SheaftException(ExceptionKind.Conflict, exception, error, objs);
        }

        public static SheaftException Forbidden(string error = null, params object[] objs)
        {
            return Forbidden(null, error, objs);
        }

        public static SheaftException Forbidden(Exception exception, string error = null, params object[] objs)
        {
            return new SheaftException(ExceptionKind.Forbidden, exception, error, objs);
        }

        public static SheaftException Gone(string error = null, params object[] objs)
        {
            return Gone(null, error, objs);
        }

        public static SheaftException Gone(Exception exception, string error = null, params object[] objs)
        {
            return new SheaftException(ExceptionKind.Gone, exception, error, objs);
        }

        public static SheaftException Locked(string error = null, params object[] objs)
        {
            return Locked(null, error, objs);
        }

        public static SheaftException Locked(Exception exception, string error = null, params object[] objs)
        {
            return new SheaftException(ExceptionKind.Locked, exception, error, objs);
        }

        public static SheaftException Unauthorized(string error = null, params object[] objs)
        {
            return Unauthorized(null, error, objs);
        }

        public static SheaftException Unauthorized(Exception exception, string error = null, params object[] objs)
        {
            return new SheaftException(ExceptionKind.Unauthorized, exception, error, objs);
        }

        public static SheaftException Unexpected(string error = null, params object[] objs)
        {
            return Unexpected(null, error, objs);
        }

        public static SheaftException Unexpected(Exception exception, string error = null, params object[] objs)
        {
            return new SheaftException(ExceptionKind.Unexpected, exception, error, objs);
        }

        public static SheaftException Validation(string error = null, params object[] objs)
        {
            return Validation(null, error, objs);
        }

        public static SheaftException Validation(Exception exception, string error = null, params object[] objs)
        {
            return new SheaftException(ExceptionKind.Validation, exception, error, objs);
        }

        public static SheaftException AlreadyExists(string error = null, params object[] objs)
        {
            return AlreadyExists(null, error, objs);
        }

        public static SheaftException AlreadyExists(Exception exception, string error = null, params object[] objs)
        {
            return new SheaftException(ExceptionKind.AlreadyExists, exception, error, objs);
        }

        public static SheaftException BadRequest(string error = null, params object[] objs)
        {
            return BadRequest(null, error, objs);
        }

        public static SheaftException BadRequest(Exception exception, string error = null, params object[] objs)
        {
            return new SheaftException(ExceptionKind.BadRequest, exception, error, objs);
        }

        public static SheaftException NotFound(string error = null, params object[] objs)
        {
            return NotFound(null, error, objs);
        }

        public static SheaftException NotFound(Exception exception, string error = null, params object[] objs)
        {
            return new SheaftException(ExceptionKind.NotFound, exception, error, objs);
        }

        public static SheaftException TooManyRetries(string error = null, params object[] objs)
        {
            return TooManyRetries(null, error, objs);
        }

        public static SheaftException TooManyRetries(Exception exception, string error = null, params object[] objs)
        {
            return new SheaftException(ExceptionKind.TooManyRetries, exception, error, objs);
        }
    }
}
