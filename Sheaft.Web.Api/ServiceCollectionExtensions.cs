using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sheaft.Application;
using Sheaft.Infrastructure;
#pragma warning disable CS8601
#pragma warning disable CS8602

namespace Sheaft.Web.Api;

public static class ServiceCollectionExtensions
{
    public static void AddCorsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(c =>
        {
            c.DefaultPolicyName = "CORS";
            c.AddPolicy("CORS", new CorsPolicy
            {
                Headers = {"*"},
                Origins = {"*"},
                Methods = {"*"}
            });
        });
    }

    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var securitySettingsConfiguration = configuration.GetSection(SecuritySettings.SECTION);
        services.Configure<SecuritySettings>(securitySettingsConfiguration);
        
        services.AddScoped<ISecuritySettings>(provider =>
            provider.GetService<IOptionsSnapshot<SecuritySettings>>().Value);

        var jwtSettingsConfiguration = configuration.GetSection(JwtSettings.SECTION);
        services.Configure<JwtSettings>(jwtSettingsConfiguration);
        
        services.AddScoped<IJwtSettings>(provider =>
            provider.GetService<IOptionsSnapshot<JwtSettings>>().Value);

        var jwtSettings = jwtSettingsConfiguration.Get<JwtSettings>();
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });
    }

    public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
    }

    public static void AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        IdentityModelEventSource.ShowPII = true;
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.AUTHENTICATED, builder =>
            {
                builder.AddAuthenticationSchemes(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    JwtBearerDefaults.AuthenticationScheme);
                builder.RequireAuthenticatedUser();
            });

            options.DefaultPolicy = options.GetPolicy(Policies.AUTHENTICATED);
        });
    }

    public static IServiceCollection RegisterSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Sheaft API",
                Version = "v1"
            });
            
            c.SwaggerGeneratorOptions.Servers = new List<OpenApiServer>()
            {
                new() {Url = "https://localhost:5003" }
            };
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                Description = "JWT containing userid claim",
                Name = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }

    public static void AddWebCommon(this IServiceCollection services, IConfiguration configuration)
    {
        var securitySettingsConfiguration = configuration.GetSection(SecuritySettings.SECTION);
        services.Configure<SecuritySettings>(securitySettingsConfiguration);
        
        services.AddScoped<ISecuritySettings>(provider =>
            provider.GetService<IOptionsSnapshot<SecuritySettings>>().Value);
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpClient();
    }
}