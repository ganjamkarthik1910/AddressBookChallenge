using System.Collections.Concurrent;
using AddressBookChallenge.Models;

namespace AddressBookChallenge.Repositories;

public class InMemoryAddressRepository : IAddressRepository
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, AddressRecord>> Store = new();

    public Task<AddressRecord> CreateAsync(AddressRecord record, CancellationToken cancellationToken)
    {
        var userStore = Store.GetOrAdd(record.UserId, _ => new ConcurrentDictionary<Guid, AddressRecord>());
        userStore[record.Id] = record;
        return Task.FromResult(record);
    }

    public Task<AddressRecord?> GetAsync(string userId, Guid addressId, CancellationToken cancellationToken)
    {
        if (Store.TryGetValue(userId, out var userStore) && userStore.TryGetValue(addressId, out var record))
        {
            return Task.FromResult<AddressRecord?>(record);
        }

        return Task.FromResult<AddressRecord?>(null);
    }

    public Task<IReadOnlyCollection<AddressRecord>> ListAsync(string userId, CancellationToken cancellationToken)
    {
        if (Store.TryGetValue(userId, out var userStore))
        {
            var result = userStore.Values.OrderBy(x => x.CreatedAtUtc).ToArray();
            return Task.FromResult<IReadOnlyCollection<AddressRecord>>(result);
        }

        return Task.FromResult<IReadOnlyCollection<AddressRecord>>(Array.Empty<AddressRecord>());
    }

    public Task<AddressRecord?> UpdateAsync(AddressRecord record, CancellationToken cancellationToken)
    {
        if (Store.TryGetValue(record.UserId, out var userStore) && userStore.ContainsKey(record.Id))
        {
            userStore[record.Id] = record;
            return Task.FromResult<AddressRecord?>(record);
        }

        return Task.FromResult<AddressRecord?>(null);
    }

    public Task<bool> DeleteAsync(string userId, Guid addressId, CancellationToken cancellationToken)
    {
        if (Store.TryGetValue(userId, out var userStore))
        {
            return Task.FromResult(userStore.TryRemove(addressId, out _));
        }

        return Task.FromResult(false);
    }
}
