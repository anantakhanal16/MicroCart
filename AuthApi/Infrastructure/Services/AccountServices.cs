using AuthApi.Application.Dtos;
using AuthApi.Application.Interface;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;

namespace AuthApi.Infrastructure.Services
{
    public class AccountServices : IAccountService
    {
        private readonly IJwtTokenHandler _tokenService;
        private readonly HttpClient _httpClient;

        public AccountServices(IJwtTokenHandler tokenService, HttpClient httpClient)
        {
            _tokenService = tokenService;
            _httpClient = httpClient;
        }

        public AuthenticationResponse? Login(AuthenticationRequest request)
        {
            return _tokenService.GenerateJwtToken(request);
        }
        public async Task<CustomerDto?> GetUserDetail(int customerId, CancellationToken cancellationToken)
        {
            Console.WriteLine("Customer Api Called");
            var response = await _httpClient.GetAsync(
                $"api/customer/{customerId}",
                cancellationToken
            );

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var customer = await response.Content.ReadFromJsonAsync<CustomerDto>(cancellationToken: cancellationToken);

            return customer;
        }
    }
}
