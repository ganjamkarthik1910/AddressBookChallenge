using System.Text.RegularExpressions;
using AddressBookChallenge.Models;

namespace AddressBookChallenge.Infrastructure;

public interface IAddressValidator
{
    void Validate(CreateAddressRequest request);
    void Validate(UpdateAddressRequest request);
}

public partial class AddressValidator : IAddressValidator
{
    public void Validate(CreateAddressRequest request)
    {
        ValidateInternal(request.Addressee, request.Street1, request.Street2, request.Town, request.County, request.Postcode);
    }

    public void Validate(UpdateAddressRequest request)
    {
        ValidateInternal(request.Addressee, request.Street1, request.Street2, request.Town, request.County, request.Postcode);
    }

    private static void ValidateInternal(string addressee, string street1, string? street2, string town, string county, string postcode)
    {
        var errors = new Dictionary<string, string[]>();

        AddRequiredAndLength(errors, nameof(addressee), addressee, required: true, maxLength: 50, "Addressee");
        AddRequiredAndLength(errors, nameof(street1), street1, required: true, maxLength: 50, "Street1");
        AddRequiredAndLength(errors, nameof(street2), street2, required: false, maxLength: 50, "Street2");
        AddRequiredAndLength(errors, nameof(town), town, required: true, maxLength: 50, "Town");
        AddRequiredAndLength(errors, nameof(county), county, required: false, maxLength: 50, "County");

        var trimmedPostcode = (postcode ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(trimmedPostcode))
        {
            errors["Postcode"] = new[] { "Postcode is required." };
        }
        else
        {
            var postcodeErrors = new List<string>();
            if (trimmedPostcode.Length > 8)
            {
                postcodeErrors.Add("Postcode must not exceed 8 characters.");
            }

            if (!UkPostcodeRegex().IsMatch(trimmedPostcode.Replace(" ", string.Empty)))
            {
                postcodeErrors.Add("Postcode must contain only letters and numbers.");
            }

            if (postcodeErrors.Count > 0)
            {
                errors["Postcode"] = postcodeErrors.ToArray();
            }
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }

    private static void AddRequiredAndLength(
        IDictionary<string, string[]> errors,
        string key,
        string? value,
        bool required,
        int maxLength,
        string displayName)
    {
        var fieldErrors = new List<string>();
        var trimmed = value?.Trim() ?? string.Empty;

        if (required && string.IsNullOrWhiteSpace(trimmed))
        {
            fieldErrors.Add($"{displayName} is required.");
        }

        if (!string.IsNullOrEmpty(trimmed) && trimmed.Length > maxLength)
        {
            fieldErrors.Add($"{displayName} must not exceed {maxLength} characters.");
        }

        if (fieldErrors.Count > 0)
        {
            errors[displayName] = fieldErrors.ToArray();
        }
    }

    [GeneratedRegex("^[A-Za-z0-9]+$", RegexOptions.Compiled)]
    private static partial Regex UkPostcodeRegex();
}
