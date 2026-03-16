using AddressBookChallenge.Models;

namespace AddressBookChallenge.Services;

public interface IAddressService
{
    Task<AddressResponse> CreateAsync(string userId, CreateAddressRequest request, CancellationToken cancellationToken);
    Task<AddressResponse> UpdateAsync(string userId, Guid id, UpdateAddressRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AddressResponse>> ListAsync(string userId, CancellationToken cancellationToken);
    Task<AddressResponse> GetAsync(string userId, Guid id, CancellationToken cancellationToken);
    Task DeleteAsync(string userId, Guid id, CancellationToken cancellationToken);
}
