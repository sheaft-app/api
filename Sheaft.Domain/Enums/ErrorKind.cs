namespace Sheaft.Domain;

public enum ErrorKind
{
    Validation,
    NotFound,
    Conflict,
    BadRequest,
    Unauthorized,
    Forbidden,
    Unexpected
}