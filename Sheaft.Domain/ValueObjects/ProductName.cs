﻿namespace Sheaft.Domain;

public record ProductName
{
    public const int MAXLENGTH = 150;
    private ProductName(){}

    public ProductName(string value)
    {
        Value = value;
    }

    public string Value { get; }
}