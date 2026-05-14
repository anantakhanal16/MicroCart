using AuthApi.Application.Dtos;
using JwtAuthenticationManager.Models;

namespace AuthApi.Application.Interface
{
    public interface IAccountService
    {
        AuthenticationResponse? Login(AuthenticationRequest request);
        Task<CustomerDto?> GetUserDetail(int customerId, CancellationToken cancellationToken); 
    }
}
