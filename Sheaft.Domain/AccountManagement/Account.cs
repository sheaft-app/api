namespace Sheaft.Domain.AccountManagement;

public class Account : AggregateRoot
{
    private List<RefreshTokenInfo> _refreshTokens = new List<RefreshTokenInfo>();
    
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
    public IReadOnlyCollection<RefreshTokenInfo> RefreshTokens => _refreshTokens.AsReadOnly();

    public Result ChangeEmail(NewEmailAddress newEmailAddress)
    {
        if (Email.Value == newEmailAddress.Value)
            return Result.Failure(ErrorKind.Validation, "change.email.new.must.differ", "The new email must be different from the old one.");

        var oldEmail = Email.Value;
        Email = newEmailAddress;

        RaiseEvent(new EmailChanged(Username.Value, oldEmail, Email.Value));
        return Result.Success();
    }

    public Result ChangePassword(ChangePassword changePassword, IPasswordHasher hasher)
    {
        var oldPasswordIsValid = hasher.PasswordIsValid(changePassword.OldPassword, Password);
        if (!oldPasswordIsValid)
            return Result.Failure(ErrorKind.Validation, "change.password.invalid.old.password", "The old password is invalid.");

        var newPasswordIsSameAsOldOne = hasher.PasswordIsValid(changePassword.NewPassword, Password);
        if (newPasswordIsSameAsOldOne)
            return Result.Failure(ErrorKind.Validation, "change.password.new.must.differ","The new password must be different from the old one.");

        var (hash, salt) = hasher.CreatePassword(changePassword.NewPassword);
        Password = HashedPassword.FromHashedString(hash, salt);
        
        RaiseEvent(new PasswordChanged(Profile.Identifier.Value));
        return Result.Success();
    }

    public Result<AuthenticationResult> Login(string password, IPasswordHasher hasher, ISecurityTokensProvider securityTokensProvider)
    {
        var passwordIsValid = hasher.PasswordIsValid(password, Password);
        if (!passwordIsValid)
            return Result.Failure<AuthenticationResult>(ErrorKind.BadRequest, "invalid.username.or.password", "Invalid password.");
        
        RaiseEvent(new AccountLoggedIn(Profile.Identifier.Value));

        return GenerateAccessToken(securityTokensProvider);
    }

    public Result<AuthenticationResult> RefreshAccessToken(RefreshTokenId refreshTokenId, ISecurityTokensProvider securityTokensProvider)
    {
        var existingToken = _refreshTokens.SingleOrDefault(rt => rt.Identifier == refreshTokenId);
        if(existingToken is {Expired: false} && existingToken.ExpiresOn > DateTimeOffset.UtcNow)
            return GenerateAccessToken(securityTokensProvider);
        
        InvalidateAllRefreshTokens();
        
        if(existingToken == null)
            return Result.Failure<AuthenticationResult>(ErrorKind.BadRequest, "refresh.token.not.found", "The refresh token used does not exists. You need to re-authenticate yourself.");
        
        if(existingToken.Expired)
            return Result.Failure<AuthenticationResult>(ErrorKind.BadRequest, "refresh.token.deactivated", "The refresh token used was already deactivated. You need to re-authenticate yourself.");
        
        if(existingToken.ExpiresOn < DateTimeOffset.UtcNow)
            return Result.Failure<AuthenticationResult>(ErrorKind.BadRequest, "refresh.token.expired", "The refresh token is expired. You need to re-authenticate yourself.");
        
        return Result.Failure<AuthenticationResult>(ErrorKind.Unexpected, "refresh.token.authentication.required", "You need to reauthenticate yourself");
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

    private Result<AuthenticationResult> GenerateAccessToken(ISecurityTokensProvider securityTokensProvider)
    {
        var refreshToken = InsertNewRefreshToken(securityTokensProvider);
        var accessToken = securityTokensProvider.GenerateAccessToken(this);

        return Result.Success(new AuthenticationResult(accessToken.Value, refreshToken, accessToken.TokenType,
            accessToken.ExpiresIn));
    }

    private string InsertNewRefreshToken(ISecurityTokensProvider securityTokensProvider)
    {
        InvalidateAllRefreshTokens();
        
        var data = securityTokensProvider.GenerateRefreshToken(Username);
        _refreshTokens.Add(data.Info);
        
        return data.Token;
    }

    private void InvalidateAllRefreshTokens()
    {
        foreach (var token in _refreshTokens)
            token.Expire();
    }
}