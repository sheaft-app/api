using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Common
{
    public class Result
    {
        internal Result(bool succeeded, List<string> errors = null, Exception exception = null)
        {
            Succeeded = succeeded;
            Exception = exception;
            Errors = errors ?? new List<string>();
            
            if (exception is AggregateException aggregateException)
                Errors.AddRange(aggregateException.InnerExceptions.Select(ie => ie.Message));
            else
            {
                var exceptionMessage = exception?.InnerException?.Message ?? exception?.Message;
                if (!string.IsNullOrWhiteSpace(exceptionMessage) && !Errors.Contains(exceptionMessage))
                    Errors.Add(exceptionMessage);
            }
        }

        public bool Succeeded { get; }

        public List<string> Errors { get; }

        public Exception Exception { get; }

        public static Result Success()
        {
            return new Result(true);
        }

        public static Result Failure(string error = null, Exception exception = null)
        {
            return new Result(false,
                new List<string> {error ?? "Une erreur inattendue est survenue pendant le traitement de la requête."},
                exception);
        }

        public static Result Failure(List<string> errors, Exception exception = null)
        {
            return new Result(false,
                errors,
                exception);
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; private set; }

        internal Result(bool succeeded, List<string> errors, Exception exception = null)
            : base(succeeded, errors, exception)
        {
        }

        public static Result<T> Success(T data, string message = null)
        {
            return new Result<T>(true, new List<string> {message}) {Data = data};
        }

        public new static Result<T> Failure(string error = null, Exception exception = null)
        {
            return new Result<T>(false,
                new List<string> {error ?? "Une erreur inattendue est survenue pendant le traitement de la requête."},
                exception);
        }

        public new static Result<T> Failure(List<string> errors, Exception exception = null)
        {
            return new Result<T>(false, errors, exception);
        }
    }
}