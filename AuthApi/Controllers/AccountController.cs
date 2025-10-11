using AuthApi.Interface;
using AuthApi.Services;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _authService;

        public AccountController(IAccountService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<AuthenticationResponse> Login([FromBody] AuthenticationRequest request)
        {
            var authenticationResponse = _authService.Login(request);
            if (authenticationResponse == null)
                return Unauthorized();

            return Ok(authenticationResponse);
        }
    }
}
