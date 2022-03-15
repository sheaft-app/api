using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sheaft.Application;
using Sheaft.Infrastructure;

namespace Sheaft.Api;

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
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "hangfire/login";
                options.LogoutPath = "hangfire/logout";
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
            options.AddPolicy(Policies.HANGFIRE, cfg =>
            {
                cfg.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                cfg.RequireAuthenticatedUser();
            });
            options.AddPolicy(Policies.AUTHENTICATED, builder =>
            {
                builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
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
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT containing userid claim",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        },
                        UnresolvedReference = true
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    public static void AddWebCommon(this IServiceCollection services, IConfiguration configuration)
    {
        var securitySettingsConfiguration = configuration.GetSection(SecuritySettings.SECTION);
        services.Configure<SecuritySettings>(securitySettingsConfiguration);
        
        services.AddScoped<ISecuritySettings>(provider =>
            provider.GetService<IOptionsSnapshot<SecuritySettings>>().Value);
        
        services.AddHttpClient();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}