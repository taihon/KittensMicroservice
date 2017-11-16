using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KittensMicroservice.Viewmodels;
using KittensMicroservice.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using KittensMicroservice.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KittensMicroservice.DataAccess.Implementation
{
    public class GenerateTokenQuery : IGenerateTokenQuery
    {
        AuthOptions Options { get; }
        public GenerateTokenQuery(IOptions<AuthOptions> options)
        {
            Options = options.Value;
        }
        public TokenResponse Run(GetTokenRequest request)
        {
            var now = DateTime.UtcNow;

            IEnumerable<Claim> customclaims = null;
            if (request.Username.StartsWith("admin"))
            {
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, request.Username));
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                customclaims = identity.Claims;
            }
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                claims: customclaims,
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256),
                expires: now.Add(TimeSpan.FromSeconds(request.ExpiresIn??10))
                );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new TokenResponse { Token = encodedJwt, Username = request.Username};
        }
    }
}
