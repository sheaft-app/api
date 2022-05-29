using System.Text.RegularExpressions;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.ProductManagement;

internal class GenerateProductCode : IGenerateProductCode
{
    private static readonly object Locker = new { };
    private readonly IDbContext _context;
    private ProductReference? _currentReference;

    public GenerateProductCode(IDbContext context)
    {
        _context = context;
    }

    public Result<ProductReference> GenerateNextCode(SupplierId supplierIdentifier)
    {
        lock (Locker)
        {
            if (_currentReference == null)
            {
                var references = _context.Set<Product>()
                    .Where(o => o.SupplierId == supplierIdentifier)
                    .OrderByDescending(o => o.Reference)
                    .Select(o => o.Reference)
                    .ToList();

                _currentReference = references
                                        .FirstOrDefault(r =>
                                            Regex.IsMatch(r.Value, ProductReference.VALIDATION_REGEX)) ??
                                    new ProductReference(0);
            }

            var nextResult = ProductReference.Next(_currentReference);
            if (nextResult.IsFailure)
                return nextResult;

            _currentReference = nextResult.Value;
            return Result.Success(_currentReference);
        }
    }
}