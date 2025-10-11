using AuthApi.Interface;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;


namespace AuthApi.Services
{
    public class AccountServices : IAccountService
    {
        private readonly IJwtTokenHandler _tokenService;

        public AccountServices(IJwtTokenHandler tokenService)
        {
            _tokenService = tokenService;
        }

        public AuthenticationResponse? Login(AuthenticationRequest request)
        {
            return _tokenService.GenerateJwtToken(request);
        }
    }
}
