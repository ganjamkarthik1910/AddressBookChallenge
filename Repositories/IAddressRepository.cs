using AddressBookChallenge.Models;

namespace AddressBookChallenge.Repositories;

public interface IAddressRepository
{
    Task<AddressRecord> CreateAsync(AddressRecord record, CancellationToken cancellationToken);
    Task<AddressRecord?> GetAsync(string userId, Guid addressId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AddressRecord>> ListAsync(string userId, CancellationToken cancellationToken);
    Task<AddressRecord?> UpdateAsync(AddressRecord record, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string userId, Guid addressId, CancellationToken cancellationToken);
}
