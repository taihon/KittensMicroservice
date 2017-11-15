using KittensMicroservice.Auth;
using KittensMicroservice.Extensions;
using KittensMicroservice.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KittensMicroservice.Middlewares
{
    public class GetTokenMiddleware
    {
        public GetTokenMiddleware(RequestDelegate next, IOptions<AuthOptions> options)
        {
            AuthOptions = options.Value;
        }
        private AuthOptions AuthOptions { get; }
        public async Task InvokeAsync(HttpContext context)
        {
            var tokenRequest = context.Request.ReadJsonAsync<GetTokenRequest>().Result;
            if (!ModelIsValid(tokenRequest))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var resp = new { error = "" };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(resp, new JsonSerializerSettings { Formatting = Formatting.Indented }));
                return;
            }
            var now = DateTime.UtcNow;
            if (tokenRequest.ExpiresIn == null)
                tokenRequest.ExpiresIn = 10;
            IEnumerable<Claim> customclaims=null;
            if (tokenRequest.Username.StartsWith("admin"))
            {
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, "hacker"));
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                customclaims = identity.Claims;
            }
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                claims: customclaims,
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256),
                expires: now.Add(TimeSpan.FromSeconds(tokenRequest.ExpiresIn.Value))
                );
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                token = encodedJwt,
                name = tokenRequest.Username
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
        private bool ModelIsValid(GetTokenRequest request)
        {
            if (request == null) return false;
            if (request.ExpiresIn < 10 || request.ExpiresIn > 300)
                return false;
            if (string.IsNullOrEmpty(request.Password) || request.Password.Length > 32)
                return false;
            if (string.IsNullOrEmpty(request.Username) || request.Username.Length > 32)
                return false;
            if (request.Username != request.Password)
                return false;
            return true;
        }
    }
}
