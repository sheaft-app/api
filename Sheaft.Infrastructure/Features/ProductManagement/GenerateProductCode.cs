using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Infrastructure.ProductManagement;

internal class GenerateProductCode : IGenerateProductCode
{
    private static int code = 0;
    public Task<Result<ProductCode>> GenerateNextProductCode(SupplierId supplierIdentifier, CancellationToken token)
    {
        code++;
        return Task.FromResult(Result.Success(new ProductCode(GenerateEanIdentifier(code, 13))));
    }

    private string GenerateEanIdentifier(long value, int length)
    {
        if (value.ToString().Length > 12)
            throw new ArgumentException("Invalid EAN length", nameof(value));

        if (length > 12)
        {
            var str = value.ToString("000000000000");
            var checksum = CalculateChecksum(str);

            return value.ToString("000000000000") + checksum;
        }

        return value.ToString("000000000000");
    }

    private int CalculateChecksum(string codeToValidate)
    {
        if (codeToValidate == null || codeToValidate.Length != 12)
            throw new ArgumentException("Code length should be 12, i.e. excluding the checksum digit",
                nameof(codeToValidate));

        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            int v;
            if (!int.TryParse(codeToValidate[i].ToString(), out v))
                throw new ArgumentException("Invalid character encountered in specified code.", nameof(codeToValidate));
            sum += (i % 2 == 0 ? v : v * 3);
        }

        int check = 10 - (sum % 10);
        return check % 10;
    }
}