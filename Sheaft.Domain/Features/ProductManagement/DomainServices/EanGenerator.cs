namespace Sheaft.Domain;

public static class EanGenerator
{
    public static string Generate(long value)
    {
        if (value.ToString().Length >= 13)
            throw new ArgumentException("Invalid EAN length", nameof(value));

        var str = value.ToString("000000000000");
        var checksum = CalculateChecksum(str);

        return value.ToString("000000000000") + checksum;
    }

    private static int CalculateChecksum(string codeToValidate)
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