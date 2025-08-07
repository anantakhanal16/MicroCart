using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtAuthenticationManager.Models;

namespace JwtAuthenticationManager
{
    public interface IJwtTokenHandler
    {
        AuthenticationResponse GenerateJwtToken(AuthenticationRequest request);
    }
    public class JwtTokenHandler: IJwtTokenHandler
    {
        public const string JwtSecurityKey = "XzA$7vH!qP4kT@eLNrWc#nF8uS6yB1mD";
        public const int JwtTokenvalidityInMinutes = 20;
        private readonly List<UserAccount> _userAccountsList;

        public JwtTokenHandler()
        {
            _userAccountsList = new List<UserAccount>
        {
            new UserAccount { UserName = "admin", Password = "admin123", Role = "Admin" },
            new UserAccount { UserName = "user", Password = "user123", Role = "User" },
        };
        }
        public AuthenticationResponse GenerateJwtToken(AuthenticationRequest request)
        {
            if(request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return null;
            }

            var user = _userAccountsList.FirstOrDefault(x => x.UserName == request.UserName && x.Password == request.Password);
            if (user == null)
            {
                return null;
            }
            var tokenExpirytime = DateTime.UtcNow.AddMinutes(JwtTokenvalidityInMinutes);
            var tokenKey= Encoding.ASCII.GetBytes(JwtSecurityKey);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("exp", tokenExpirytime.ToString())
            };

            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(tokenKey),
                Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256
            );
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpirytime,
                SigningCredentials = signingCredentials
            };

            var SecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = SecurityTokenHandler.CreateToken(tokenDescriptor);
            var token = SecurityTokenHandler.WriteToken(securityToken);

            // Generate JWT token logic here
            return new AuthenticationResponse
            {
                JwtToken = token,
                UserName = user.UserName,
                ExpiryIn =(int)tokenExpirytime.Subtract(DateTime.Now).TotalSeconds
            };
        }
    }
}
