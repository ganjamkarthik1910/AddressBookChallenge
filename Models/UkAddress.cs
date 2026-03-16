namespace AddressBookChallenge.Models;

public class UkAddress
{
    // Required, maximum length 50 characters
    // Example: Mrs. Elizabeth White
    public string Addressee { get; set; } = string.Empty;
    
    // Required, maximum length 50 characters
    // Example: Hathaway Cottage
    public string Street1 { get; set; } = string.Empty;
    
    // Optional, maximum length 50 characters
    // Example: 1 Main Street
    public string? Street2 { get; set; } = null;
    
    // Required, maximum length 50 characters
    public string Town { get; set; } = string.Empty;
    
    // Optional, maximum length 50 characters
    public string County { get; set; } = string.Empty;
    
    // Required, maximum length 8 characters, only letters and numbers are allowed
    public string Postcode { get; set; } = string.Empty;
}
