using KittensMicroservice.Models;
using KittensMicroservice.Options;
using KittensMicroservice.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KittensMicroservice.Middlewares
{
    public class GetKittensMiddleware
    {
        public GetKittensMiddleware(RequestDelegate next)
        {

        }
        public async Task InvokeAsync(HttpContext ctx, IDataService dataService)
        {
            IEnumerable<Kitten> kittens = await dataService.GetDataAsync();
            await ctx.Response.WriteAsync(
                JsonConvert.SerializeObject(kittens, JsonSerializerOptions.GetOptions())
                );
        }
    }
}
