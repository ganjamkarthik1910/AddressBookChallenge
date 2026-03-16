using AddressBookChallenge.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookChallenge.Controllers;

[ApiController]
[Route("dev")]
public class DevelopmentController : ControllerBase
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IDevelopmentTokenService _tokenService;

    public DevelopmentController(IHostEnvironment hostEnvironment, IDevelopmentTokenService tokenService)
    {
        _hostEnvironment = hostEnvironment;
        _tokenService = tokenService;
    }

    [HttpGet("token/{userId}")]
    public IActionResult GetToken(string userId)
    {
        if (!_hostEnvironment.IsDevelopment())
        {
            return NotFound();
        }

        var token = _tokenService.CreateToken(userId);
        return Ok(new
        {
            access_token = token,
            token_type = "Bearer"
        });
    }
}
