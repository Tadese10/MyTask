using System.Text;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Database;
using Infrastructure.Time;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;
using MongoDB.EntityFrameworkCore;
using MongoDB.Driver;
using Infrastructure.Database.MongoDb;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Common;
using Newtonsoft.Json;
using StackExchange.Redis;
using Infrastructure.Caching;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using DnsClient.Internal;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddServices(configuration)
            .AddMongoDbContext<MongoDbContext>(configuration)
            //.AddDatabase(configuration)
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration)
            .AddAuthorizationInternal();

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.Configure<RedisOptions>(configuration.GetSection(nameof(RedisOptions)));

        //Redis Configuration
        ILoggerFactory loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole());
        RedisOptions? _option = configuration.GetRequiredSection(nameof(RedisOptions)).Get<RedisOptions>();
        var config = ConfigurationOptions.Parse($"{_option!.Url}", true);
        config.Password = _option!.Password;
        config.User = _option!.Username;
        config.ResolveDns = true;
        config.ConnectRetry = 100;
        config.ConnectTimeout = 60 * 60 * 1000 * 10;
        config.AbortOnConnectFail = false;
        config.LoggerFactory = loggerFactory;
         services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(config));

        return services;
    }

    //private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    //{
    //    string connectionString = configuration.GetConnectionString("Database") ?? "";
    //    //string useMongoDb = configuration["Database"];

    //    services.AddDbContext<ApplicationDbContext>(
    //       options => options
    //           .UseMongoDB(connectionString, "TestDatabase")
    //           .UseSnakeCaseNamingConvention());

    //    services.AddScoped<IApplicationDbContextMongoDb>(sp => sp.GetRequiredService<ApplicationDbContextMongoDb>());

    //    services.AddMongoDbContext<Database.MongoDb.MongoDbContext>(configuration);

    //    return services;
    //}

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!);

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };

                o.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;

                        string? userId = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

                        context.HttpContext?.Items.Add(Constants.UserIDKey, userId);
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        string result = JsonConvert.SerializeObject(new Response<string>
                        {
                            Code = 401,
                            Successful = false,
                            Message = "You are not Authorized"
                        });
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        string result = JsonConvert.SerializeObject(new Response<string>
                        {
                            Code = 403,
                            Message = "You are not authorized to access this resource",
                            Successful = false
                        });
                        return context.Response.WriteAsync(result);
                    }
                };
            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddScoped<PermissionProvider>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}
