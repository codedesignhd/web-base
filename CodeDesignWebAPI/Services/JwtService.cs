using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using CodeDesign.BL;
using CodeDesign.Models;
using CodeDesign.Utilities.Constants;
using Microsoft.IdentityModel.Tokens;

namespace CodeDesign.WebAPI.Services
{
    internal class JwtService
    {
        public string GenAccessToken(Account user, int expiredMins = 30)
        {
            if (user is null)
            {
                return string.Empty;
            }

            string secret = Utilities.ConfigurationManager.AppSettings["Jwt:Key"];
            byte[] symmetricKey = Encoding.UTF8.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim(ClaimTypesCustom.Id, user.id),
                        new Claim(ClaimTypes.Name, user.username),
                        new Claim(ClaimTypes.Role, Convert.ToString(user.role)),
                        new Claim(ClaimTypes.Email, user.email),
                        new Claim(ClaimTypesCustom.Username, user.username),
                        new Claim(ClaimTypes.GivenName, user.fullname),
                        new Claim(ClaimTypesCustom.Properties, string.Join(",", user.thuoc_tinh)),
                    }),

                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expiredMins)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = CodeDesign.Utilities.ConfigurationManager.AppSettings["Jwt:Issuer"],
                Audience = Utilities.ConfigurationManager.AppSettings["Jwt:Audience"],
            };

            SecurityToken stoken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(stoken);

            return token;
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            long createdDate = Utilities.DateTimeUtils.TimeInEpoch();
            long expires = Utilities.DateTimeUtils.TimeInEpoch(DateTime.UtcNow.AddDays(7));
            RefreshToken refreshToken = new RefreshToken
            {
                token = getUniqueToken(),
                expires = expires,
                created_date = createdDate,
                ip = ipAddress
            };
            return refreshToken;
        }

        private string getUniqueToken(int maxCall = 10)
        {
            string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            if (maxCall <= 0)
            {
                return token;
            }
            bool tokenIsUnique = AccountBL.Instance.IsUniqueRefreshToken(token);
            if (!tokenIsUnique)
                return getUniqueToken(maxCall - 1);
            return token;
        }
        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                byte[] symmetricKey = Convert.FromBase64String(Utilities.ConfigurationManager.AppSettings["Jwt:Key"]);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            catch (Exception)
            {
                //should write log
                return null;
            }
        }

        private bool isValidToken(string token, out string username)
        {
            username = string.Empty;

            var simplePrinciple = GetPrincipal(token);
            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null || !identity.IsAuthenticated)
                return false;

            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            // More validate to check whether username exists in system

            return true;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            if (isValidToken(token, out string username))
            {
                // based on username to get more information from database 
                Account tk = AccountBL.Instance.Get(username, new string[] { "username", "email", "role" });
                if (tk != null)
                {
                    List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, tk.username),
                    new Claim(ClaimTypes.GivenName, tk.fullname),
                    new Claim(ClaimTypes.Role,Convert.ToString((int) tk.role)),
                };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Jwt");
                    IPrincipal user = new ClaimsPrincipal(identity);

                    return Task.FromResult(user);
                }
            }
            return Task.FromResult<IPrincipal>(null);
        }
    }
}
