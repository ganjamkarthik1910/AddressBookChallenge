using AddressBookChallenge.Auth;
using AddressBookChallenge.Models;
using AddressBookChallenge.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookChallenge.Controllers;

[ApiController]
[Authorize]
[Route("api/addresses")]
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<AddressesController> _logger;

    public AddressesController(
        IAddressService addressService,
        ICurrentUserService currentUserService,
        ILogger<AddressesController> logger)
    {
        _addressService = addressService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AddressResponse>> Create([FromBody] CreateAddressRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        _logger.LogInformation("Create address requested by user {UserId}", userId);

        var response = await _addressService.CreateAsync(userId, request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AddressResponse>> Update(Guid id, [FromBody] UpdateAddressRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        var response = await _addressService.UpdateAsync(userId, id, request, cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<AddressResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyCollection<AddressResponse>>> List(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        var response = await _addressService.ListAsync(userId, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AddressResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        var response = await _addressService.GetAsync(userId, id, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        await _addressService.DeleteAsync(userId, id, cancellationToken);
        return NoContent();
    }
}
