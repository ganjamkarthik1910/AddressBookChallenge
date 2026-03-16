namespace AddressBookChallenge.Models;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "AddressBookChallenge";
    public string Audience { get; set; } = "AddressBookChallenge.Client";
    public string SigningKey { get; set; } = "development-signing-key-change-me";
    public int ExpiryMinutes { get; set; } = 60;
}
