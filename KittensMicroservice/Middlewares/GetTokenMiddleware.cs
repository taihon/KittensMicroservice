using KittensMicroservice.DataAccess;
using KittensMicroservice.Extensions;
using KittensMicroservice.Options;
using KittensMicroservice.Viewmodels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace KittensMicroservice.Middlewares
{
    public class GetTokenMiddleware
    {
        public GetTokenMiddleware(RequestDelegate next)
        {
            
        }
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
                await context.Response.WriteAsync(JsonConvert.SerializeObject(resp, JsonSerializerOptions.GetOptions()));
                return;
            }
            var response = query.Run(tokenRequest);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, JsonSerializerOptions.GetOptions()));
        }
    }
}
