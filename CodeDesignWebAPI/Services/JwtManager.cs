using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using CodeDesign.BL;
using CodeDesign.Dtos;
using CodeDesign.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;

namespace CodeDesign.WebAPI.Services
{
    public class JwtManager
    {
        public static string GenToken(Account user, int expired_mins = 30)
        {
            if (user != null)
            {
                string secret = Utilities.ConfigurationManager.AppSettings["Jwt:Key"];
                byte[] symmetricKey = Encoding.UTF8.GetBytes(secret);
                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, user.username),
                        new Claim(ClaimTypes.Role, Convert.ToString(user.role)),
                        new Claim(ClaimTypes.Email, user.email),
                        new Claim(ClaimTypesCustom.USERNAME, user.username),
                        new Claim(ClaimTypes.GivenName, user.fullname),
                        new Claim(ClaimTypesCustom.THUOC_TINH, string.Join(",", user.thuoc_tinh)),
                    }),

                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expired_mins)),

                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey),
                        SecurityAlgorithms.HmacSha256Signature),
                    Issuer = CodeDesign.Utilities.ConfigurationManager.AppSettings["Jwt:Issuer"],
                    Audience = Utilities.ConfigurationManager.AppSettings["Jwt:Audience"],
                };

                SecurityToken stoken = tokenHandler.CreateToken(tokenDescriptor);
                string token = tokenHandler.WriteToken(stoken);

                return token;
            }
            return string.Empty;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Utilities.ConfigurationManager.AppSettings["Jwt:Key"]);

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

        private static bool isValidToken(string token, out string username)
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
