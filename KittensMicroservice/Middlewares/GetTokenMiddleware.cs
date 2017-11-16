using KittensMicroservice.Auth;
using KittensMicroservice.DataAccess;
using KittensMicroservice.Extensions;
using KittensMicroservice.Viewmodels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public async Task InvokeAsync(HttpContext context, IGenerateTokenQuery query)
        {
            var tokenRequest = await context.Request.ReadJsonAsync<GetTokenRequest>();
            context.Response.ContentType = "application/json";
            ValidationContext validationContext = new ValidationContext(tokenRequest, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(tokenRequest,validationContext,results,true))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var resp = new { error = "", errors = results };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(resp, new JsonSerializerSettings { Formatting = Formatting.Indented }));
                return;
            }
            var response = query.Run(tokenRequest);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
    }
}
