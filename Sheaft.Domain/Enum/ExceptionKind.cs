namespace Sheaft.Domain.Enum
{
    public enum ExceptionKind
    {
        Validation,
        BadRequest,
        Conflict,
        Unauthorized,
        Forbidden,
        Locked,
        NotFound,
        Gone,
        Unexpected,
        AlreadyExists,
        TooManyRetries,
    }
}