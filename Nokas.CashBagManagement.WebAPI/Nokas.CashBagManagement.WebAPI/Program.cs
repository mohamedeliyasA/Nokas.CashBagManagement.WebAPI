using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using Nokas.CashBagManagement.WebAPI;
using Nokas.CashBagManagement.WebAPI.Configuration;
using Nokas.CashBagManagement.WebAPI.Middleware;
using Nokas.CashBagManagement.WebAPI.Repository;
using Nokas.CashBagManagement.WebAPI.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Logging

builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

#endregion

#region Core Infrastructure

builder.Services.AddAzureAdJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
})
.AddNewtonsoftJson()
.AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(config =>
{
    config.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

#endregion

#region Dependency Injection

builder.Services.AddSingleton(serviceProvider =>
{
    var config = builder.Configuration.GetSection("CosmosDb");
    return new CosmosClient(config["Account"], config["Key"]);
});

builder.Services.AddScoped<IBagRegistrationRepo, CosmosBagRegistrationRepo>();
builder.Services.AddSingleton<IBlobArchiveService, BlobArchiveService>();
builder.Services.AddSingleton<IServiceBusSender, ServiceBusSender>();

#endregion

var app = builder.Build();

#region Middleware

app.UseMiddleware<ExceptionLoggingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

#endregion

#region Swagger + Environment-Specific Rules

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CashBag Management API V1");
    });
}

app.UseSwaggerRestrictions(app.Environment);

#endregion

app.MapControllers();
app.Run();
