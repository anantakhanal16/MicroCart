using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager.Models
{
    public class AuthenticationResponse
    {
        public string JwtToken { get; set; }
        public string UserName { get; set; }
        public int ExpiryIn { get; set; }
    }
}
