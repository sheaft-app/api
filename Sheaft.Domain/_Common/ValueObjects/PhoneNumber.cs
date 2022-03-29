using System;
using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record PhoneNumber
{
    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException(nameof(value));

        if (!Regex.IsMatch(value, @"^(?:(?:\+|00)33|0)\s*[1-9](?:[\s.-]*\d{2}){4}$"))
            throw new InvalidOperationException("Phone number is invalid");

        Value = value;
    }

    public string Value { get; }
}