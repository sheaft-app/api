using System.Text.RegularExpressions;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.ProductManagement;

internal class GenerateReturnableCode : IGenerateReturnableCode
{
    private static readonly object Locker = new { };
    private readonly IDbContext _context;
    private ReturnableReference? _currentReference;

    public GenerateReturnableCode(IDbContext context)
    {
        _context = context;
    }

    public Result<ReturnableReference> GenerateNextCode(SupplierId supplierIdentifier)
    {
        lock (Locker)
        {
            if (_currentReference == null)
            {
                var references = _context.Set<Returnable>()
                    .Where(o => o.SupplierId == supplierIdentifier)
                    .OrderByDescending(o => o.Reference)
                    .Select(o => o.Reference)
                    .ToList();

                _currentReference = references
                                        .FirstOrDefault(r =>
                                            Regex.IsMatch(r.Value, ReturnableReference.VALIDATION_REGEX)) ??
                                    new ReturnableReference(0);
            }

            var nextResult = ReturnableReference.Next(_currentReference);
            if (nextResult.IsFailure)
                return nextResult;

            _currentReference = nextResult.Value;
            return Result.Success(_currentReference);
        }
    }
}