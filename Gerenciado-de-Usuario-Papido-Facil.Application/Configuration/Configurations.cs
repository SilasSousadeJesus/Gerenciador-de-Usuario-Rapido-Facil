
using Microsoft.IdentityModel.Tokens;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Configuration
{
    public class Configurations
    {
        public class JwtOptions
        {
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public SigningCredentials SigningCredentials { get; set; }
            public int AccessTokenExpiration { get; set; }
            public int RefreshTokenExpiration { get; set; }
        }
    }
}
