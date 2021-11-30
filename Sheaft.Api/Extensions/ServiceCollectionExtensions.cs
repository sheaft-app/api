using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sheaft.Api.Security;
using Sheaft.Application.Configurations;
using Sheaft.Domain.Security;

namespace Sheaft.Api.Extensions
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
            services
                .AddAuthentication(options =>
                {
                    // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                });
            // .AddJwtBearer(options =>
            // {
            //     options.Events = new JwtBearerEvents
            //     {
            //         OnMessageReceived = context =>
            //         {
            //             var accessToken = context.Request.Query["access_token"];
            //
            //             // If the request is for our hub...
            //             var path = context.HttpContext.Request.Path;
            //             if (!string.IsNullOrEmpty(accessToken) &&
            //                 (path.StartsWithSegments("/hubs/sheaft")))
            //             {
            //                 // Read the token out of the query string
            //                 context.Token = accessToken;
            //             }
            //             return Task.CompletedTask;
            //         },
            //         OnAuthenticationFailed = context =>
            //         {
            //             return Task.CompletedTask;
            //         },
            //         OnChallenge = context =>
            //         {
            //             return Task.CompletedTask;
            //         },
            //         OnForbidden = context =>
            //         {
            //             return Task.CompletedTask;
            //         },
            //         OnTokenValidated = context =>
            //         {
            //             return Task.CompletedTask;
            //         }
            //     };
            // });
        }

        public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheSettings = configuration.GetSection(CacheConfiguration.SETTING);
            var databaseSettings = configuration.GetSection(AppDatabaseConfiguration.SETTING);
            
            services.Configure<CacheConfiguration>(cacheSettings);
            var cacheConfig = cacheSettings.Get<CacheConfiguration>();
            
            services.Configure<AppDatabaseConfiguration>(databaseSettings);
            var databaseConfig = cacheSettings.Get<AppDatabaseConfiguration>();
            
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
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.HANGFIRE, cfg =>
                {
                    cfg.AddRequirements().RequireAuthenticatedUser();
                    cfg.AddAuthenticationSchemes("oidc");
                    cfg.AddRequirements().RequireRole(RoleDefinition.Admin, RoleDefinition.Support);
                });
                options.AddPolicy(Policies.AUTHENTICATED, builder => builder.RequireAuthenticatedUser());
                options.AddPolicy(Policies.REGISTERED, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Supplier, RoleDefinition.Store, RoleDefinition.Consumer);
                });
                options.AddPolicy(Policies.STORE_OR_PRODUCER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Supplier, RoleDefinition.Store);
                });
                options.AddPolicy(Policies.STORE_OR_CONSUMER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Store, RoleDefinition.Consumer);
                });
                options.AddPolicy(Policies.UNREGISTERED, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Anonymous);
                });
                options.AddPolicy(Policies.OWNER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Owner);
                });
                options.AddPolicy(Policies.PRODUCER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Supplier);
                });
                options.AddPolicy(Policies.STORE, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Store);
                });
                options.AddPolicy(Policies.CONSUMER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleDefinition.Consumer);
                });
                options.AddPolicy(Policies.ANONYMOUS_OR_CONNECTED, builder =>
                {
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