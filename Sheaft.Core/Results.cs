using System;

namespace Sheaft.Core
{
    public class Result<T> : Result
    {
        public T Data { get; private set; }

        internal Result(bool succeeded, string message, Exception exception = null, params object[] objs) : base(succeeded, message, exception, objs)
        {
        }
        
        public static Result<T> Success(T data, string message = null, params object[] objs)
        {
            return new Result<T>(true, message, null, objs) {Data = data};
        }

        public static Result<T> Failure(T data, string error, params object[] objs)
        {
            return new Result<T>(false, error, null, objs);
        }

        public static Result<T> Failure(T data, string error, Exception exception, params object[] objs)
        {
            return new Result<T>(false, error, exception, objs);
        }
    }
}