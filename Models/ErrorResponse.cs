namespace AddressBookChallenge.Models;

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string? TraceId { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
}
