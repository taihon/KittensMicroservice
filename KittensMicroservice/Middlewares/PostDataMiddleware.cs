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
            if (context.User.IsInRole("admin")) { 
                var kitten = await context.Request.ReadJsonAsync<Kitten>();
                kitten.CreatedBy = context.User.Identity.Name;
                await dataService.SaveDataAsync(kitten);
                context.Response.StatusCode = StatusCodes.Status204NoContent;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
        }
    }
}
