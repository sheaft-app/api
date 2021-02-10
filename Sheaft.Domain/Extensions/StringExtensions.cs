namespace Sheaft.Domain.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNotNullAndIsEmptyOrWhiteSpace(this string s)
        {
            return s != null && s.Replace(" ", "").Length == 0;
        }
    }
}
