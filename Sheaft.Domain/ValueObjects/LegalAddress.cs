﻿namespace Sheaft.Domain;

public record LegalAddress(string Street, string? Complement, string Postcode, string City) 
    : Address(Street, Complement, Postcode, City);