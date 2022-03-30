﻿namespace Sheaft.Domain.AccountManagement;

public interface ISecurityTokensProvider
{
    AccessToken GenerateAccessToken(Account account, string? profileIdentifier);
    RefreshToken GenerateRefreshToken(AccountId accountIdentifier);
    (AccountId, RefreshTokenId) RetrieveTokenIdentifierData(string refreshToken);
}