using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Sheaft.Api.Security;

namespace Sheaft.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCorsServices(this IServiceCollection services, IConfiguration configuration)
        {
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