﻿using KittensMicroservice.Models;
using KittensMicroservice.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KittensMicroservice.Middlewares
{
    public class GetDataMiddleware
    {
        public GetDataMiddleware(RequestDelegate next)
        {

        }
        public async Task InvokeAsync(HttpContext ctx, IDataService dataService)
        {
            IEnumerable<Kitten> kittens = await dataService.GetDataAsync();
            await ctx.Response.WriteAsync(
                JsonConvert.SerializeObject(kittens)
                );
        }
    }
}
