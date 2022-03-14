namespace Sheaft.Application;

public interface IJwtSettings
{
    string Issuer { get; set; }
    string Secret { get; set; }
    string Salt { get; set; }
}