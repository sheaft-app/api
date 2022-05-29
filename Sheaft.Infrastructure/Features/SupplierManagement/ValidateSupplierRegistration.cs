﻿using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.SupplierManagement;

internal class ValidateSupplierRegistration : IValidateSupplierRegistration
{
    private readonly IDbContext _context;

    public ValidateSupplierRegistration(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> CanRegisterAccount(AccountId identifier, CancellationToken token)
    {
        try
        {
            return Result.Success(
                await _context.Set<Supplier>().AllAsync(s => s.AccountId != identifier, token));
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }
}