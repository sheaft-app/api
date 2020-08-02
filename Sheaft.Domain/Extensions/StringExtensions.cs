using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Sheaft.Domains.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNotNullAndIsEmptyOrWhiteSpace(this string s)
        {
            return s != null && s.Replace(" ", "").Length == 0;
        }
    }
}
