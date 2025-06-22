using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nokas.CashBagManagement.WebAPI;
using Nokas.CashBagManagement.WebAPI.DBContext;
using Nokas.CashBagManagement.WebAPI.Middleware;
using Nokas.CashBagManagement.WebAPI.Repository;
using Nokas.CashBagManagement.WebAPI.Services;
using Serilog;





var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Configure Azure AD settings
var azureAD = builder.Configuration.GetSection("AzureAd");

#region authentication
// Configure authentication with JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = azureAD["Authority"];
        options.Audience = azureAD["Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = azureAD["ValidIssuer"],
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
        // Log invalid token attempts(authentication failed)
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                // Log details of the failed authentication attempt
                var correlationId = context.HttpContext?.Items["CorrelationId"]?.ToString() ?? "N/A";
                Log.Warning("CorrelationId: {CorrelationId} - Authentication failed. Invalid token: {Exception}", correlationId, context.Exception.Message);

                // Optionally, add additional information about the exception (expired token, invalid signature, etc.)
                return Task.CompletedTask;
            }
        };
    });

#endregion

#region json and xml support
// Add controllers with JSON and XML support

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
})
.AddNewtonsoftJson(options =>
{
    //forcing to get the camelcase
    options.SerializerSettings.ContractResolver =
        new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
})
.AddXmlSerializerFormatters();


#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#region Services Dependency Injection    
builder.Services.AddSingleton<BagRegistrationDataStore>();
builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var config = builder.Configuration.GetSection("CosmosDb");
    return new CosmosClient(config["Account"], config["Key"]);
});
//builder.Services.AddSingleton<IBagRegistrationRepo, CosmosBagRegistrationRepo>();

//builder.Services.AddDbContext<BagRegistrationDBContext>(dbContextOptions =>
//                                dbContextOptions
//                                .UseSqlServer(
//        builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

builder.Services.AddScoped<IBagRegistrationRepo, CosmosBagRegistrationRepo>();

builder.Services.AddSingleton<IBlobArchiveService, BlobArchiveService>();
builder.Services.AddSingleton<IServiceBusSender, ServiceBusSender>();
#endregion

var app = builder.Build();

#region Middlewares
app.UseMiddleware<ExceptionLoggingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
