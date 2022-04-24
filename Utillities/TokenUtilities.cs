using CriticalConditionBackend.CriticalConditionExceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CriticalConditionBackend.Utillities
{
    public static class TokenUtilities
    {
        public static string CreateToken(IConfiguration Configuration, List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                    issuer: Configuration["JWT:ValidIssuer"],
                    audience: Configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static JwtSecurityToken ValidateAndReturnToken(string token, IConfiguration Configuration)
        {
            token = token.Replace("Bearer ", string.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, GetTokenValidationParameters(Configuration), out _);

                return tokenHandler.ReadJwtToken(token);
            }
            catch
            {
                throw new LogicalException(CriticalConditionExceptionsEnum.TOKEN_USED_IS_NOT_VALID, StatusCodes.Status401Unauthorized);
            }
        }

        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration Configuration)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["JWT:ValidIssuer"],
                ValidAudience = Configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
            };
            return tokenValidationParameters;
        }
    }
}
