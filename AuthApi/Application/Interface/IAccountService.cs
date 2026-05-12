using JwtAuthenticationManager.Models;

namespace AuthApi.Application.Interface
{
    public interface IAccountService
    {
        AuthenticationResponse? Login(AuthenticationRequest request);
    }
}
