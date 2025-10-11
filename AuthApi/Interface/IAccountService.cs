using JwtAuthenticationManager.Models;

namespace AuthApi.Interface
{
    public interface IAccountService
    {
        AuthenticationResponse? Login(AuthenticationRequest request);
    }
}
