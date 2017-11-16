using KittensMicroservice.DataAccess;
using KittensMicroservice.Extensions;
using KittensMicroservice.Models;
using KittensMicroservice.Services;
using KittensMicroservice.Viewmodels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KittensMicroservice.Middlewares
{
    public class AddKittenMiddleware
    {
        public AddKittenMiddleware(RequestDelegate next)
        {

        }
        public async Task InvokeAsync(HttpContext context, ICreateKittenCommand command)
        {
            if (context.User.IsInRole("admin")) { 
                var request = await context.Request.ReadJsonAsync<CreateKittenRequest>();
                await command.ExecuteAsync(request, context.User.Identity.Name);
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
