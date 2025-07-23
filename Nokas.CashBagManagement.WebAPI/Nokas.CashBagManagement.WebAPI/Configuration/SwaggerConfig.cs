using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Nokas.CashBagManagement.WebAPI.Configuration
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CashBag Management API",
                    Version = "v1",
                    Description = "API for secure bag registration, updates, and lifecycle tracking.",
                    Contact = new OpenApiContact
                    {
                        Name = "Nokas Support",
                        Email = "support@nokas.no"
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @" Enter your Azure AD access token below.

To get a token, send this POST request to Azure AD:

POST <your-Token URL>

With form fields:
- grant_type=client_credentials
- client_id=<your-clientid>
- client_secret=<your-secret>
- scope=<your-scope>

Paste the token below ( no 'Bearer ' prefix).",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });
        }

        public static void UseSwaggerRestrictions(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsProduction())
                return;

            app.Use(async (context, next) =>
            {
                var method = context.Request.Method;
                var referer = context.Request.Headers["Referer"].ToString();
                var isSwagger = referer.Contains("/swagger", StringComparison.OrdinalIgnoreCase);
                var isModifying = HttpMethods.IsPost(method) || HttpMethods.IsPut(method) || HttpMethods.IsDelete(method);

                if (isSwagger && isModifying)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Swagger UI is restricted from modifying data in production. Use API clients like Postman instead.");
                    return;
                }

                await next();
            });
        }
    }
}
