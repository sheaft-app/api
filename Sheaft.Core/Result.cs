using System;
using Sheaft.Core.Enums;

namespace Sheaft.Core
{
    public class Result
    {
        internal Result(bool succeeded, string message, Exception exception = null, params object[] objs)
        {
            Succeeded = succeeded;
            Message = message;
            Exception = exception;
            Params = objs;
        }

        public bool Succeeded { get; }
        public string Message { get; }
        public Exception Exception { get; }
        public object[] Params { get; }

        public static Result Success(string message = null, params object[] objs)
        {
            return new Result(true, message ?? "Requête executée avec succès.", null, objs);
        }

        public static Result Failure(string error, params object[] objs)
        {
            return new Result(false, error ?? "Une erreur est survenue pendant le traitement de la requête.", null, objs);
        }

        public static Result Failure(string error, Exception exception, params object[] objs)
        {
            return new Result(false, error ?? "Une erreur inattendue est survenue pendant le traitement de la requête.", exception, objs);
        }
    }
}