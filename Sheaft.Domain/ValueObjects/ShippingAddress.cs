﻿namespace Sheaft.Domain;

public record ShippingAddress(string Name, EmailAddress Email, string Street, string? Complement, string Postcode, string City) 
    : Address(Street, Complement, Postcode, City);