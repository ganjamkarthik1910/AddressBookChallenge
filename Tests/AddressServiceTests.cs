using AddressBookChallenge.Infrastructure;
using AddressBookChallenge.Models;
using AddressBookChallenge.Repositories;
using AddressBookChallenge.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AddressBookChallenge.Tests;

public class AddressServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateAddress()
    {
        var repository = new InMemoryAddressRepository();
        var validator = new AddressValidator();
        var service = new AddressService(repository, validator, NullLogger<AddressService>.Instance);

        var request = new CreateAddressRequest
        {
            Addressee = "Mrs. Elizabeth White",
            Street1 = "Hathaway Cottage",
            Street2 = "1 Main Street",
            Town = "Stratford-upon-Avon",
            County = "Warwickshire",
            Postcode = "CV376AA"
        };

        var result = await service.CreateAsync("user-1", request, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("user-1", (await repository.GetAsync("user-1", result.Id, CancellationToken.None))?.UserId);
        Assert.Equal("CV376AA", result.Postcode);
    }

    [Fact]
    public async Task GetAsync_ShouldNotReturnOtherUsersAddress()
    {
        var repository = new InMemoryAddressRepository();
        var validator = new AddressValidator();
        var service = new AddressService(repository, validator, NullLogger<AddressService>.Instance);

        var created = await service.CreateAsync("user-1", new CreateAddressRequest
        {
            Addressee = "Mrs. Elizabeth White",
            Street1 = "Hathaway Cottage",
            Town = "Stratford-upon-Avon",
            County = "Warwickshire",
            Postcode = "CV376AA"
        }, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetAsync("user-2", created.Id, CancellationToken.None));
    }
}
