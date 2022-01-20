using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Sheaft.Api.Security;
using Sheaft.Application.Configurations;
using Sheaft.Domain.Security;

namespace Sheaft.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCorsServices(this IServiceCollection services, IConfiguration configuration)
        {
            var corsConfig = configuration.GetSection(CorsConfiguration.SETTING).Get<CorsConfiguration>();
            services.AddCors(options =>
            {
                options.AddPolicy("cors",
                    builder =>
                    {
                        builder.WithOrigins(corsConfig.Origins.ToArray())
                            .WithHeaders(corsConfig.Headers.ToArray())
                            .WithMethods(corsConfig.Methods.ToArray());
                    });
            });
        }
        
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration, Constants.AzureAdB2C)
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddInMemoryTokenCaches();
            
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
            });
            
            services.AddAuthentication("OpenIdConnect")
                .AddMicrosoftIdentityWebApp(configuration, Constants.AzureAdB2C);

            services.Configure<MicrosoftIdentityOptions>(options =>
            {
                options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;
                options.UsePkce = true;
                options.ResponseType = OpenIdConnectResponseType.Code;
            });
        }

        public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheSettings = configuration.GetSection(CacheConfiguration.SETTING);
            
            services.Configure<AppDatabaseConfiguration>(configuration.GetSection(AppDatabaseConfiguration.SETTING));
            var databaseSettings = configuration.GetSection(AppDatabaseConfiguration.SETTING);
            
            services.Configure<CacheConfiguration>(cacheSettings);
            var cacheConfig = cacheSettings.Get<CacheConfiguration>();
            
            services.Configure<AppDatabaseConfiguration>(databaseSettings);
            var databaseConfig = databaseSettings.Get<AppDatabaseConfiguration>();
            
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = databaseConfig.ConnectionString;
                options.SchemaName = cacheConfig.SchemaName;
                options.TableName = cacheConfig.TableName;
            });
            
            services.AddMemoryCache();
        }
        
        public static void AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.HANGFIRE, cfg =>
                {
                    cfg.AddAuthenticationSchemes("OpenIdConnect");
                    cfg.AddRequirements().RequireAuthenticatedUser();
                });
                options.AddPolicy(Policies.AUTHENTICATED, builder =>
                {
                    builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    builder.RequireAuthenticatedUser();
                });
                options.AddPolicy(Policies.REGISTERED, builder =>
                {
                    builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Supplier, RoleDefinition.Store, RoleDefinition.Consumer);
                });
                options.AddPolicy(Policies.STORE_OR_PRODUCER, builder =>
                {
                    builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Supplier, RoleDefinition.Store);
                });
                options.AddPolicy(Policies.STORE_OR_CONSUMER, builder =>
                {
                    builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Store, RoleDefinition.Consumer);
                });
                options.AddPolicy(Policies.UNREGISTERED, builder =>
                {
                    builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Anonymous);
                });
                options.AddPolicy(Policies.OWNER, builder =>
                {
                    builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Owner);
                });
                options.AddPolicy(Policies.PRODUCER, builder =>
                {
                    builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Supplier);
                });
                options.AddPolicy(Policies.STORE, builder =>
                {
                    builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Store);
                });
                options.AddPolicy(Policies.CONSUMER, builder =>
                {
                    builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Consumer);
                });
                options.AddPolicy(Policies.ANONYMOUS_OR_CONNECTED, builder =>
                {
                    builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    builder.RequireAssertion(c => c.User.Identity is not {IsAuthenticated: true} || c.User.Identity.IsAuthenticated);
                });
                
                options.DefaultPolicy = options.GetPolicy(Policies.ANONYMOUS_OR_CONNECTED);
            });
        }
        
        public static void AddWebCommon(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }
    }
}