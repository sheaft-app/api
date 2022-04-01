namespace Sheaft.Domain;

public record NewEmailAddress : EmailAddress
{
    public NewEmailAddress(string newValue, string confirmEmailAddress)
        : base(newValue)
    {
        if (newValue != confirmEmailAddress)
            throw new InvalidOperationException("Email and confirmation are different");
    }
}