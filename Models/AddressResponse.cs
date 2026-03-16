namespace AddressBookChallenge.Models;

public class AddressResponse
{
    public Guid Id { get; set; }
    public string Addressee { get; set; } = string.Empty;
    public string Street1 { get; set; } = string.Empty;
    public string? Street2 { get; set; }
    public string Town { get; set; } = string.Empty;
    public string County { get; set; } = string.Empty;
    public string Postcode { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
}
