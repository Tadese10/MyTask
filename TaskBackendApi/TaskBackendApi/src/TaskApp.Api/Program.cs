using System.Reflection;
using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Serilog;
using TaskApp.Api;
using TaskApp.Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#pragma warning disable CA1305 // Specify IFormatProvider
builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console());
#pragma warning restore CA1305 // Specify IFormatProvider

builder.Logging.ClearProviders().AddConsole().AddDebug();

builder.Services.AddSwaggerGenWithAuth();

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();


app.MapEndpoints();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwaggerWithUi();

//    app.ApplyMigrations();
//}

app.UseSwaggerWithUi();

app.ApplyMigrations();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseCors(options =>
               options.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod());

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

// REMARK: If you want to use Controllers, you'll need this.
app.MapControllers();

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
namespace TaskApp.Api
{
    public partial class Program;
}
