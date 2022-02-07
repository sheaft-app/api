using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Common
{
    public class ValidationError
    {
        public string Identifier { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum ResultStatus
    {
        Ok,
        Error,
        Forbidden,
        Unauthorized,
        Invalid,
        NotFound
    }

    public interface IResult
    {
        bool IsSuccess { get; }
        ResultStatus Status { get; }
        string Message { get; }
        IEnumerable<string> Errors { get; }
        List<ValidationError> ValidationErrors { get; }
    }

    public class Result : IResult
    {
        protected Result()
        {
        }

        protected Result(ResultStatus status)
        {
            Status = status;
        }

        public ResultStatus Status { get; } = ResultStatus.Ok;
        public bool IsSuccess => Status == ResultStatus.Ok;
        public string Message { get; protected set; } = string.Empty;
        public IEnumerable<string> Errors { get; protected set; } = new List<string>();
        public List<ValidationError> ValidationErrors { get; protected set; } = new List<ValidationError>();

        public static Result Success()
        {
            return new Result();
        }

        public static Result Success(string successMessage)
        {
            return new Result {Message = successMessage};
        }

        public static Result Error(Exception exception)
        {
            return new Result(ResultStatus.Error) { Message = exception.Message };
        }

        public static Result Error(params string[] errorMessages)
        {
            return new Result(ResultStatus.Error) {Errors = errorMessages, Message = errorMessages.Length > 1 ? "Multiple errors occured." : errorMessages.FirstOrDefault()};
        }

        public static Result Invalid(List<ValidationError> validationErrors)
        {
            return new Result(ResultStatus.Invalid) {ValidationErrors = validationErrors, Message = "Error(s) occured while applying validations."};
        }

        public static Result NotFound()
        {
            return new Result(ResultStatus.NotFound){Message = "Resource not found."};
        }

        public static Result Forbidden()
        {
            return new Result(ResultStatus.Forbidden){Message = "Resource access is not allowed."};
        }

        public static Result Unauthorized()
        {
            return new Result(ResultStatus.Unauthorized){Message = "Resource requires authenticated access."};
        }
    }

    public class Result<T> : Result
    {
        public Result(T value)
        {
            Value = value;
        }

        private Result(ResultStatus status)
            : base(status)
        {
        }

        public static implicit operator T(Result<T> result) => result.Value;
        public static implicit operator Result<T>(T value) => Success(value);

        public T Value { get; }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Success(T value, string successMessage)
        {
            return new Result<T>(value) {Message = successMessage};
        }
        
        public new static Result<T> Success()
        {
            return new Result<T>(ResultStatus.Ok);
        }

        public new static Result<T> Error(Exception e)
        {
            return new Result<T>(ResultStatus.Error) {Message = e.Message};
        }

        public new static Result<T> Error(params string[] errorMessages)
        {
            return new Result<T>(ResultStatus.Error) {Errors = errorMessages, Message = errorMessages.Length > 1 ? "Multiple errors occured." : errorMessages.FirstOrDefault()};
        }

        public new static Result<T> Invalid(List<ValidationError> validationErrors)
        {
            return new Result<T>(ResultStatus.Invalid) {ValidationErrors = validationErrors, Message = "Error(s) occured while applying validations."};
        }

        public new static Result<T> NotFound()
        {
            return new Result<T>(ResultStatus.NotFound){Message = "Resource not found."};
        }

        public new static Result<T> Forbidden()
        {
            return new Result<T>(ResultStatus.Forbidden){Message = "Resource access is not allowed."};
        }

        public new static Result<T> Unauthorized()
        {
            return new Result<T>(ResultStatus.Unauthorized){Message = "Resource requires authenticated access."};
        }
    }
}