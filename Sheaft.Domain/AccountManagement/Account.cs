using Sheaft.Domain.ProfileManagement;

namespace Sheaft.Domain.AccountManagement;

public class Account : AggregateRoot
{
    private List<RefreshToken> _refreshTokens = new List<RefreshToken>();
    
    private Account()
    {
    }

    public Account(Username username, EmailAddress email, HashedPassword password, Profile profile)
    {
        Username = username;
        Email = email;
        Password = password;
        Profile = profile;
    }
    
    public Username Username { get; private set; }
    public EmailAddress Email { get; private set; }
    public HashedPassword Password { get; private set; }
    public ResetPasswordInfo? ResetPasswordInfo { get; private set; }
    public Profile Profile { get; private set; }
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    public Result ChangeEmail(NewEmailAddress newEmailAddress)
    {
        if (Email.Value == newEmailAddress.Value)
            return Result.Failure(ErrorKind.Validation, "The new email must be different from the old one.");

        var oldEmail = Email.Value;
        Email = newEmailAddress;

        RaiseEvent(new EmailChanged(Username.Value, oldEmail, Email.Value));
        return Result.Success();
    }

    public Result ChangePassword(ChangePassword changePassword, IPasswordHasher hasher)
    {
        var oldPasswordIsValid = hasher.PasswordIsValid(changePassword.OldPassword, Password);
        if (!oldPasswordIsValid)
            return Result.Failure(ErrorKind.Validation, "The old password is invalid.");

        var newPasswordIsSameAsOldOne = hasher.PasswordIsValid(changePassword.NewPassword, Password);
        if (newPasswordIsSameAsOldOne)
            return Result.Failure(ErrorKind.Validation, "The new password must be different from the old one.");

        var (hash, salt) = hasher.CreatePassword(changePassword.NewPassword);
        Password = HashedPassword.FromHashedString(hash, salt);
        
        RaiseEvent(new PasswordChanged(Profile.Identifier.Value));
        return Result.Success();
    }

    public Result<AuthenticationToken> Login(string password, IPasswordHasher hasher, ISecurityTokensProvider securityTokensProvider)
    {
        var passwordIsValid = hasher.PasswordIsValid(password, Password);
        if (!passwordIsValid)
            return Result.Failure<AuthenticationToken>(ErrorKind.BadRequest, "invalid.username.or.password", "Invalid password.");
        
        RaiseEvent(new AccountLoggedIn(Profile.Identifier.Value));

        return GenerateAccessToken(securityTokensProvider);
    }

    public Result<AuthenticationToken> RefreshAccessToken(RefreshTokenId refreshTokenId, ISecurityTokensProvider securityTokensProvider)
    {
        var existingToken = _refreshTokens.SingleOrDefault(rt => rt.Identifier == refreshTokenId);
        if(existingToken is {Expired: false} && existingToken.ExpiresOn > DateTimeOffset.UtcNow)
            return GenerateAccessToken(securityTokensProvider);
        
        InvalideAllRefreshTokens();
        
        if(existingToken == null)
            return Result.Failure<AuthenticationToken>(ErrorKind.BadRequest, "The refresh token used does not exists. You need to re-authenticate yourself.");
        
        if(existingToken.Expired)
            return Result.Failure<AuthenticationToken>(ErrorKind.BadRequest, "The refresh token used was already deactivated. You need to re-authenticate yourself.");
        
        if(existingToken.ExpiresOn < DateTimeOffset.UtcNow)
            return Result.Failure<AuthenticationToken>(ErrorKind.BadRequest, "The refresh token is expired. You need to re-authenticate yourself.");
        
        return Result.Failure<AuthenticationToken>(ErrorKind.Unexpected, "You need to re-authenticate yourself.");
    }

    public Result ForgotPassword(DateTimeOffset currentDate, int tokenValidityInHours)
    {
        if (tokenValidityInHours < 1)
            throw new InvalidOperationException("Reset password token validity in hours must be greater or equal than 1 hour.");
        
        ResetPasswordInfo = new ResetPasswordInfo(Guid.NewGuid().ToString("N"), currentDate.AddHours(tokenValidityInHours));
        RaiseEvent(new PasswordForgotten(Profile.Identifier.Value, ResetPasswordInfo.Token, ResetPasswordInfo.ExpiresOn));

        return Result.Success();
    }

    public Result ResetPassword(string token, NewPassword newPassword, IPasswordHasher hasher)
    {
        if (string.IsNullOrWhiteSpace(ResetPasswordInfo?.Token))
            return Result.Failure(ErrorKind.BadRequest, "requires.forgot.password.call");
        
        if (ResetPasswordInfo.ExpiresOn < DateTimeOffset.UtcNow)
            return Result.Failure(ErrorKind.BadRequest, "reset.password.token.expired");

        if(ResetPasswordInfo.Token != token)
            return Result.Failure(ErrorKind.BadRequest, "invalid.reset.password.token");
        
        ResetPasswordInfo = null;
        Password = HashedPassword.Create(newPassword.Value, hasher);
        
        RaiseEvent(new PasswordReset(Profile.Identifier.Value));
        return Result.Success();
    }

    private Result<AuthenticationToken> GenerateAccessToken(ISecurityTokensProvider securityTokensProvider)
    {
        var refreshToken = InsertNewRefreshToken(securityTokensProvider);
        var accessToken = securityTokensProvider.GenerateAccessToken(this);

        return Result.Success(new AuthenticationToken(accessToken.Value, refreshToken, accessToken.TokenType,
            accessToken.ExpiresIn));
    }

    private string InsertNewRefreshToken(ISecurityTokensProvider securityTokensProvider)
    {
        InvalideAllRefreshTokens();
        
        var (data, refreshToken) = securityTokensProvider.GenerateRefreshToken(Username);
        _refreshTokens.Add(data);
        
        return refreshToken;
    }

    private void InvalideAllRefreshTokens()
    {
        foreach (var token in _refreshTokens)
            token.Expire();
    }
}