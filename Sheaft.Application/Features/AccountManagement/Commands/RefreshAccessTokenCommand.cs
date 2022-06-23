using FluentValidation;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Application.AccountManagement;

public record RefreshAccessTokenCommand(string RefreshToken) : Command<Result<AuthenticationTokenDto>>;

internal class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
        RuleFor(c => c.RefreshToken).NotEmpty().WithMessage("RefreshToken is required");
    }
}

internal class RefreshAccessTokenHandler : ICommandHandler<RefreshAccessTokenCommand, Result<AuthenticationTokenDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ISecurityTokensProvider _securityTokensProvider;
    private readonly IRetrieveProfile _retrieveProfile;

    public RefreshAccessTokenHandler(
        IUnitOfWork uow,
        ISecurityTokensProvider securityTokensProvider,
        IRetrieveProfile retrieveProfile)
    {
        _uow = uow;
        _securityTokensProvider = securityTokensProvider;
        _retrieveProfile = retrieveProfile;
    }

    public async Task<Result<AuthenticationTokenDto>> Handle(RefreshAccessTokenCommand request, CancellationToken token)
    {
        var (username, refreshTokenId) = _securityTokensProvider.RetrieveTokenIdentifierData(request.RefreshToken);
        var accountResult = await _uow.Accounts.Get(username, token);
        if (accountResult.IsFailure)
            return Result.Failure<AuthenticationTokenDto>(accountResult);
        
        var account = accountResult.Value;
        var accountProfileResult = await _retrieveProfile.GetAccountProfile(account.Id, token);
        if (accountProfileResult.IsFailure)
            return Result.Failure<AuthenticationTokenDto>(accountProfileResult);

        var refreshResult = account.RefreshAccessToken(refreshTokenId, _securityTokensProvider, accountProfileResult.Value.HasValue ? accountProfileResult.Value.Value : null);
        if (refreshResult.IsFailure)
            return Result.Failure<AuthenticationTokenDto>(refreshResult);

        _uow.Accounts.Update(account);
        var result = await _uow.Save(token);
        
        return result.IsSuccess
            ? Result.Success(new AuthenticationTokenDto(refreshResult.Value.AccessToken, refreshResult.Value.RefreshToken, refreshResult.Value.TokenType, refreshResult.Value.ExpiresIn))
            : Result.Failure<AuthenticationTokenDto>(result);
    }
}