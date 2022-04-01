﻿using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.AccountManagement;

internal class AccountRepository : Repository<Account, AccountId>, IAccountRepository
{
    public AccountRepository(IDbContext context)
        : base(context)
    {
    }

    public Task<Result<Maybe<Account>>> FindByEmail(EmailAddress email, CancellationToken token)
    {
        return QueryAsync(async () =>
            Result.Success(await Values
                .Include(c => c.RefreshTokens)
                .SingleOrDefaultAsync(e => e.Email == email, token) ?? Maybe<Account>.None));
    }

    public override Task<Result<Account>> Get(AccountId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .Include(c => c.RefreshTokens)
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);
            
            return result != null
                ? Result.Success(result)
                : Result.Failure<Account>(ErrorKind.NotFound, "account.not.found");
        });
    }

    public Task<Result<Account>> Get(Username username, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .Include(c => c.RefreshTokens)
                .SingleOrDefaultAsync(e => e.Username == username, token);
            
            return result != null
                ? Result.Success(result)
                : Result.Failure<Account>(ErrorKind.NotFound, "account.not.found");
        });
    }
}