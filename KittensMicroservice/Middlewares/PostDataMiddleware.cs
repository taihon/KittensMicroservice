using KittensMicroservice.Extensions;
using KittensMicroservice.Models;
using KittensMicroservice.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KittensMicroservice.Middlewares
{
    public class PostDataMiddleware
    {
        public PostDataMiddleware(RequestDelegate next)
        {

        }
        public async Task InvokeAsync(HttpContext context, IDataService dataService)
        {
            var kitten = await context.Request.ReadJsonAsync<Kitten>();
            await dataService.SaveDataAsync(kitten);
        }
    }
}
