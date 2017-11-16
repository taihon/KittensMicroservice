using KittensMicroservice.Options;
using KittensMicroservice.Viewmodels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KittensMicroservice.DataAccess.Implementation
{
    public class GenerateTokenQuery : IGenerateTokenQuery
    {
        AuthOptions Options { get; }
        public GenerateTokenQuery()
        {

        }
        public TokenResponse Run(GetTokenRequest request)
        {
            var now = DateTime.UtcNow;

            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, request.Username));
            if (request.Username.StartsWith("admin"))
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                claims: identity.Claims,
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256),
                expires: now.Add(TimeSpan.FromSeconds(request.ExpiresIn??10))
                );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new TokenResponse { Token = encodedJwt };
        }
    }
}
