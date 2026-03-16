using AddressBookChallenge.Infrastructure;
using AddressBookChallenge.Models;
using AddressBookChallenge.Repositories;

namespace AddressBookChallenge.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _repository;
    private readonly IAddressValidator _validator;
    private readonly ILogger<AddressService> _logger;

    public AddressService(IAddressRepository repository, IAddressValidator validator, ILogger<AddressService> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<AddressResponse> CreateAsync(string userId, CreateAddressRequest request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var record = new AddressRecord
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Address = new UkAddress
            {
                Addressee = request.Addressee.Trim(),
                Street1 = request.Street1.Trim(),
                Street2 = request.Street2?.Trim(),
                Town = request.Town.Trim(),
                County = request.County?.Trim() ?? string.Empty,
                Postcode = request.Postcode.Trim().ToUpperInvariant()
            },
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        var created = await _repository.CreateAsync(record, cancellationToken);
        _logger.LogInformation("Created address {AddressId} for user {UserId}", created.Id, userId);

        return Map(created);
    }

    public async Task<AddressResponse> UpdateAsync(string userId, Guid id, UpdateAddressRequest request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var existing = await _repository.GetAsync(userId, id, cancellationToken);
        if (existing is null)
        {
            throw new NotFoundException($"Address '{id}' was not found for the current user.");
        }

        existing.Address = new UkAddress
        {
            Addressee = request.Addressee.Trim(),
            Street1 = request.Street1.Trim(),
            Street2 = request.Street2?.Trim(),
            Town = request.Town.Trim(),
            County = request.County?.Trim() ?? string.Empty,
            Postcode = request.Postcode.Trim().ToUpperInvariant()
        };
        existing.UpdatedAtUtc = DateTimeOffset.UtcNow;

        var updated = await _repository.UpdateAsync(existing, cancellationToken);
        if (updated is null)
        {
            throw new NotFoundException($"Address '{id}' was not found for the current user.");
        }

        _logger.LogInformation("Updated address {AddressId} for user {UserId}", id, userId);
        return Map(updated);
    }

    public async Task<IReadOnlyCollection<AddressResponse>> ListAsync(string userId, CancellationToken cancellationToken)
    {
        var records = await _repository.ListAsync(userId, cancellationToken);
        return records.Select(Map).ToArray();
    }

    public async Task<AddressResponse> GetAsync(string userId, Guid id, CancellationToken cancellationToken)
    {
        var record = await _repository.GetAsync(userId, id, cancellationToken);
        if (record is null)
        {
            throw new NotFoundException($"Address '{id}' was not found for the current user.");
        }

        return Map(record);
    }

    public async Task DeleteAsync(string userId, Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(userId, id, cancellationToken);
        if (!deleted)
        {
            throw new NotFoundException($"Address '{id}' was not found for the current user.");
        }

        _logger.LogInformation("Deleted address {AddressId} for user {UserId}", id, userId);
    }

    private static AddressResponse Map(AddressRecord record)
    {
        return new AddressResponse
        {
            Id = record.Id,
            Addressee = record.Address.Addressee,
            Street1 = record.Address.Street1,
            Street2 = record.Address.Street2,
            Town = record.Address.Town,
            County = record.Address.County,
            Postcode = record.Address.Postcode,
            CreatedAtUtc = record.CreatedAtUtc,
            UpdatedAtUtc = record.UpdatedAtUtc
        };
    }
}
