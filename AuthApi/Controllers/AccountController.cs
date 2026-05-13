using AuthApi.Application.Interface;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _authService;

        public AccountController(IAccountService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<AuthenticationResponse> Login([FromBody] AuthenticationRequest request)
        {
            Console.WriteLine("Login Api Called ");
            var authenticationResponse = _authService.Login(request);
            Console.WriteLine("Login user Detail");
            if (authenticationResponse == null)
                return Unauthorized();

            return Ok(authenticationResponse);
        }

        [HttpGet("getUserDetails")]
        public async  Task<ActionResult<AuthenticationResponse>> GetUserDetails([FromQuery] int customerId)
        {
            Console.WriteLine("from Auth Api Getting User Detail Called");
            var userDetail =  await _authService.GetUserDetail(customerId, CancellationToken.None);
            return Ok(userDetail);
        }
    }
}
