using KittensMicroservice.DataAccess;
using KittensMicroservice.DataAccess.Implementation;
using KittensMicroservice.Extensions;
using KittensMicroservice.Middlewares;
using KittensMicroservice.Options;
using KittensMicroservice.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace KittensMicroservice
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<KittensContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddScoped<IDataService, DataService>();
            // Register queries and commands
            services
                .AddScoped<ICreateKittenCommand, CreateKittenCommand>()
                .AddScoped<IGenerateTokenQuery, GenerateTokenQuery>()
                .AddScoped<IKittensListQuery, KittensListQuery>()
                ;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Audience,
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        LifetimeValidator = (DateTime? notBefore, DateTime? expires,
                            SecurityToken token, TokenValidationParameters parameters) =>
                                {
                                    if (expires != null)
                                        return DateTime.UtcNow < expires;
                                    return false;
                                }
                    };
                }
                );
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                ;
            if (env.IsDevelopment())
                builder.AddUserSecrets<Startup>();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, KittensContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            dbContext.Database.Migrate();
            app.UseStaticFiles();
            app.Use(ValidateMethod);
            app.UseAuthentication();
            app.Map("/token", _ => _.UseMiddleware<GetTokenMiddleware>());
            app.UseMiddleware<IsAuthenticatedMiddleware>();
            app.MapMethod(HttpMethods.Get, _ => _.UseMiddleware<GetKittensMiddleware>());
            app.MapMethod(HttpMethods.Post, _ => _.UseMiddleware<AddKittenMiddleware>());
        }
        private Task ValidateMethod(HttpContext context, Func<Task> next)
        {
            if (HttpMethods.IsGet(context.Request.Method) ||
                HttpMethods.IsPost(context.Request.Method))
            {
                return next();
            }
            context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
            return Task.CompletedTask;
        }
    }
}
