using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtTokenHandler _tokenService;
        public AccountController(IJwtTokenHandler tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public  ActionResult<AuthenticationResponse> Login([FromBody] AuthenticationRequest request)
        {
            var authenticationResponse =  _tokenService.GenerateJwtToken(request);
            if (authenticationResponse == null)
            {
                return  Unauthorized();
            }
            return Ok(authenticationResponse);
        }
    }
}
