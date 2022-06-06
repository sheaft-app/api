﻿namespace Sheaft.Domain.AccountManagement;

public class Account : AggregateRoot
{
    private List<RefreshTokenInfo> _refreshTokens = new List<RefreshTokenInfo>();
    
    private Account()
    {
    }

    public Account(Username username, EmailAddress email, HashedPassword password, Firstname firstname, Lastname lastname)
    {
        Id = AccountId.New();
        Username = username;
        Email = email;
        Password = password;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
        SetNames(firstname, lastname);
    }

    public AccountId Id { get; }
    public Username Username { get; private set; }
    public Firstname Firstname { get; private set; }
    public Lastname Lastname { get; private set; }
    public EmailAddress Email { get; private set; }
    public HashedPassword Password { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }
    public ResetPasswordInfo? ResetPasswordInfo { get; private set; }
    public IReadOnlyCollection<RefreshTokenInfo> RefreshTokens => _refreshTokens.AsReadOnly();

    public Result ChangeEmail(NewEmailAddress newEmailAddress)
    {
        if (Email.Value == newEmailAddress.Value)
            return Result.Failure(ErrorKind.Validation, "change.email.new.must.differ", "The new email must be different from the old one.");

        var oldEmail = Email.Value;
        Email = newEmailAddress;
        UpdatedOn = DateTimeOffset.UtcNow;

        RaiseEvent(new EmailChanged(Id.Value, oldEmail, Email.Value));
        return Result.Success();
    }
    
    public void SetNames(Firstname firstname, Lastname lastname)
    {
        Firstname = firstname;
        Lastname = lastname;
        UpdatedOn = DateTimeOffset.UtcNow;
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
        UpdatedOn = DateTimeOffset.UtcNow;
        
        RaiseEvent(new PasswordChanged(Id.Value));
        return Result.Success();
    }

    public Result<AuthenticationResult> Login(string password, IPasswordHasher hasher, ISecurityTokensProvider securityTokensProvider, Profile profile)
    {
        var passwordIsValid = hasher.PasswordIsValid(password, Password);
        if (!passwordIsValid)
            return Result.Failure<AuthenticationResult>(ErrorKind.BadRequest, "invalid.username.or.password", "Invalid password.");
        
        RaiseEvent(new AccountLoggedIn(Id.Value));

        return GenerateAccessToken(securityTokensProvider, profile);
    }

    public Result<AuthenticationResult> RefreshAccessToken(RefreshTokenId refreshTokenId, ISecurityTokensProvider securityTokensProvider, Profile profile)
    {
        var existingToken = _refreshTokens.SingleOrDefault(rt => rt.Identifier == refreshTokenId);
        if(existingToken is {Expired: false} && existingToken.ExpiresOn > DateTimeOffset.UtcNow)
            return GenerateAccessToken(securityTokensProvider, profile);
        
        InvalidateAllRefreshTokens();
        
        if(existingToken == null)
            return Result.Failure<AuthenticationResult>(ErrorKind.BadRequest, "refresh.token.not.found", "The refresh token used does not exists. You need to re-authenticate yourself.");
        
        if(existingToken.Expired)
            return Result.Failure<AuthenticationResult>(ErrorKind.BadRequest, "refresh.token.deactivated", "The refresh token used was already deactivated. You need to re-authenticate yourself.");
        
        if(existingToken.ExpiresOn < DateTimeOffset.UtcNow)
            return Result.Failure<AuthenticationResult>(ErrorKind.BadRequest, "refresh.token.expired", "The refresh token is expired. You need to re-authenticate yourself.");
        
        return Result.Failure<AuthenticationResult>(ErrorKind.Unexpected, "refresh.token.authentication.required", "You need to reauthenticate yourself");
    }

    public void ForgotPassword(DateTimeOffset currentDate, int tokenValidityInHours)
    {
        if (tokenValidityInHours < 1)
            throw new InvalidOperationException("Reset password token validity in hours must be greater or equal than 1 hour.");
        
        ResetPasswordInfo = new ResetPasswordInfo(Guid.NewGuid().ToString("N"), DateTimeOffset.UtcNow, currentDate.AddHours(tokenValidityInHours));
        UpdatedOn = DateTimeOffset.UtcNow;
        
        RaiseEvent(new PasswordForgotten(Id.Value, ResetPasswordInfo.Token, ResetPasswordInfo.ExpiresOn));
    }

    public Result ResetPassword(string token, NewPassword newPassword, IPasswordHasher hasher)
    {
        if (string.IsNullOrWhiteSpace(ResetPasswordInfo?.Token))
            return Result.Failure(ErrorKind.BadRequest, "reset.password.no.token.info");
        
        if (ResetPasswordInfo.ExpiresOn < DateTimeOffset.UtcNow)
            return Result.Failure(ErrorKind.BadRequest, "reset.password.token.expired");

        if(ResetPasswordInfo.Token != token)
            return Result.Failure(ErrorKind.BadRequest, "reset.password.invalid.token");
        
        ResetPasswordInfo = null;
        Password = HashedPassword.Create(newPassword.Value, hasher);
        UpdatedOn = DateTimeOffset.UtcNow;
        
        RaiseEvent(new PasswordReset(Id.Value));
        return Result.Success();
    }

    private Result<AuthenticationResult> GenerateAccessToken(ISecurityTokensProvider securityTokensProvider, Profile? profile)
    {
        var refreshToken = InsertNewRefreshToken(securityTokensProvider);
        var accessToken = securityTokensProvider.GenerateAccessToken(this, profile);

        return Result.Success(new AuthenticationResult(accessToken.Value, refreshToken, accessToken.TokenType,
            accessToken.ExpiresIn));
    }

    private string InsertNewRefreshToken(ISecurityTokensProvider securityTokensProvider)
    {
        InvalidateAllRefreshTokens();
        
        var data = securityTokensProvider.GenerateRefreshToken(Id);
        _refreshTokens.Add(data.Info);
        
        return data.Token;
    }

    private void InvalidateAllRefreshTokens()
    {
        foreach (var token in _refreshTokens)
            token.Expire();
    }
}