using System;
using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record PhoneNumber
{
    public PhoneNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentNullException(nameof(number));

        if (!Regex.IsMatch(number, @"^(?:(?:\+|00)33|0)\s*[1-9](?:[\s.-]*\d{2}){4}$"))
            throw new InvalidOperationException("Phone number is invalid");

        Number = number;
    }

    public string Number { get; }
}