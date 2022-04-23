﻿using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.AgreementManagement;

public class ValidateAgreementProposal : IValidateAgreementProposal
{
    private readonly IDbContext _context;

    public ValidateAgreementProposal(IDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<bool>> CanCreateAgreementBetween(string requester, string receiver, CancellationToken token)
    {
        try
        {
            var activeAgreementExists = await _context.Set<Agreement>().AnyAsync(a =>
                (a.Status == AgreementStatus.Active || a.Status == AgreementStatus.Pending)
                && (a.CustomerIdentifier == new CustomerId(receiver) &&
                    a.SupplierIdentifier == new SupplierId(requester))
                || (a.CustomerIdentifier == new CustomerId(requester) &&
                    a.SupplierIdentifier == new SupplierId(receiver)), token);
            
            return Result.Success(!activeAgreementExists);
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }
}