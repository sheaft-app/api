using System.Runtime.CompilerServices;

namespace Sheaft.Domain;

/// <summary>
/// This code is inspired from CSharpFunctionalExtensions nuget written by vladimir khorikov
/// </summary>
/// <typeparam name="T"></typeparam>
public record Result
{
    private readonly Error _error;
    protected Result() => IsSuccess = true;

    protected Result(Error error) => _error = error;

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error
    {
        get
        {
            if (IsSuccess)
                throw new InvalidOperationException("Cannot access Error property if in success state.");

            return _error;
        }
    }

    public static Result Success() => new Result();
    public static Result<T> Success<T>(T data) => new Result<T>(data);

    public static Result SuccessIf<T>(Result<T> result)
    {
        return result.IsSuccess ? Success() : Failure(result);
    }
    public static Result<T> SuccessIf<T>(Result result, T data)
    {
        return result.IsSuccess ? Success<T>(data) : Failure<T>(result);
    }
    public static Result<T> SuccessIf<TU, T>(Result<TU> result, T data)
    {
        return result.IsSuccess ? Success<T>(data) : Failure<T>(result);
    }
    public static Result<T> SuccessIf<T>(bool isSuccess, T data, Error error)
    {
        return isSuccess ? Success<T>(data) : Failure<T>(error);
    }
    public static Result Failure() => Failure(ErrorKind.Unexpected, "error.unexpected");
    public static Result<T> Failure<T>() => Failure<T>(ErrorKind.Unexpected, "error.unexpected");
    public static Result<T> Failure<T>(Error error) => new Result<T>(error);
    public static Result<T> Failure<T>(Result errorResult) => new Result<T>(errorResult.Error);
    
    public static Result<T> Failure<T>(ErrorKind kind, string code, string? message = null, Dictionary<string, object>? extensions = null, [CallerMemberName] string? callerName = null, [CallerLineNumber] int? callerLine = null) 
        => new Result<T>(new Error(kind, code, message, extensions, $"{callerName}:{callerLine}")); public static Result Failure(Result errorResult) => new Result(errorResult.Error);
    
    public static Result Failure(ErrorKind kind, string code, string? message = null, Dictionary<string, object>? extensions = null, [CallerMemberName] string? callerName = null, [CallerLineNumber] int? callerLine = null) 
        => new Result(new Error(kind, code, message, extensions, $"{callerName}:{callerLine}"));

    public static Result<T> CombineFailure<T>(List<Result<T>> results)
    {
        if (results.Any(r => r.IsSuccess))
            throw new InvalidOperationException("Can only combine failed results");

        var extensions = new Dictionary<string, object>
        {
            {"errors", new List<object>()}
        };
        
        foreach (var result in results)
        {
            if (extensions.TryGetValue("errors", out var errors))
                ((List<object>)errors).Add(new {result.Error.Code, result.Error.Message});

            if (result.Error.Extensions == null)
                continue;
            
            foreach (var extension in result.Error.Extensions)
            {
                if (!extensions.TryGetValue(extension.Key, out var existingExtension))
                    extensions.Add(extension.Key, existingExtension = new List<object>{extension.Value});
                else
                    ((List<object>)existingExtension).Add(extension.Value);
            }
        }
        
        return Result.Failure<T>(ErrorKind.BadRequest, "one.or.multiple.errors.occured", "One or multiple errors occured.", extensions);
    }
}

public record Result<T> : Result
{
    private readonly T _value;
    internal Result(T value) => _value = value;
    internal Result(Error error) : base(error){}

    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException("Cannot access Value property if in failed state.");
            
            return _value;
        }
    }
}

public record Error(ErrorKind Kind, string Code, string? Message = null, Dictionary<string, object>? Extensions = null, string? Caller = null);